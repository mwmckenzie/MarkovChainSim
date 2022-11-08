// Markov Chain Sim -- Adjuster.cs
// 
// Copyright (C) 2022 Matthew W. McKenzie and Kenz LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using Sirenix.OdinInspector;

namespace MarkovChainModel
{
    public class Adjuster : MarkovModule, IReceiver
    {
        [Title("Connector to Adjust")] [HideLabel] [Required]
        public Connector adjustableConnector;

        public bool adjustBaselineProbability;

        [Title("Signal In")] [HideLabel] [Required]
        public Connector connectorIn;

        [Title("Adjustment Type")] [HideLabel] public AdjustmentType adjustmentType;

        [Title("Adjustment Function")] [HideLabel, InlineEditor] [Required]
        public AdjustmentFunction adjustmentFunction;

        [Title("Values")] public float signal;
        public float adjustedProbability;

        private void Start()
        {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            RegisterAsReceiver();
            adjustmentFunction.ResetHistory();
        }

        protected override void OnValidate()
        {
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast()
        {
            history.Add(adjustedProbability);
            signal = 0f;
        }

        private void Adjust(float val)
        {
            signal = val;
            adjustedProbability = adjustmentFunction.Adjust(signal);
            //adjustableConnector.probability = adjustedProbability;

            var priorProb = adjustableConnector.probability;

            switch (adjustmentType)
            {
                case AdjustmentType.Replace:
                    break;
                case AdjustmentType.Sum:
                    adjustedProbability += priorProb;
                    break;
                case AdjustmentType.Multiply:
                    adjustedProbability *= priorProb;
                    break;
                case AdjustmentType.Divide:
                    if (adjustedProbability != 0f)
                    {
                        adjustedProbability = priorProb / adjustedProbability;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // adjustableConnector.probability = adjustedProbability;
            // if (adjustBaselineProbability) {
            //     adjustableConnector.baselineProbability = adjustedProbability;
            // }

            adjustableConnector.AdjustProbability(adjustedProbability, adjustBaselineProbability);
        }

        public void Receive(float val)
        {
            Adjust(val);
        }

        public void RegisterAsReceiver()
        {
            connectorIn.RegisterReciever(this);
        }
    }

    public enum AdjustmentType
    {
        Replace,
        Sum,
        Multiply,
        Divide
    }
}