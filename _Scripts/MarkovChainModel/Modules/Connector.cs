// Markov Chain Sim -- Connector.cs
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

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MarkovChainModel
{
    public class Connector : MarkovModule, ISensible
    {
        [Title("Connections")] 
        public ISender sender;
        public IReceiver receiver;

        [Title("Probabilities")] 
        public float baselineProbability = 1f;
        public float probability = 1f;

        [Title("Clamped Flow")] 
        public bool clamped = true;

        [Title("Values")] 
        public float flowMeter;

        [Title("Probability History")] 
        public List<float> historyProbability;

        protected override void OnValidate()
        {
            if (TrySetSenderFromParent())
            {
                SetNameFromConnections();
                var senderModule = (MarkovModule)sender;
                cleanUpBroadcast = senderModule.cleanUpBroadcast;
            }
            else
            {
                SetNameIfNothing();
            }
            ClampProbabilities();
        }

        private bool TrySetSenderFromParent()
        {
            var module = GetComponentInParent<ISender>();
            if (module is null)
            {
                return false;
            }
            sender = module;
            return true;
        }

        private void SetNameFromConnections()
        {
            var senderModule = (MarkovModule)sender;
            var senderModuleName = senderModule.name;
            var receiverModuleName = string.Empty;
            if (receiver != null)
            {
                var receiverModule = (MarkovModule)receiver;
                receiverModuleName = receiverModule.name;
            }
            name = $"{senderModuleName}To{receiverModuleName}";
            gameObject.name = name;
        }

        private void Start()
        {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            ResetProbabilities();
            if (clamped)
            {
                ClampProbabilities();
            }
        }

        private void OnCleanUpBroadcast()
        {
            ResetProbabilities();
            if (clamped)
            {
                ClampProbabilities();
            }
        }

        private void SetNameIfNothing()
        {
            if (name.IsNullOrWhitespace())
            {
                name = gameObject.name;
            }
        }

        private void ResetProbabilities()
        {
            probability = baselineProbability;
        }

        private void ClampProbabilities()
        {
            baselineProbability = Mathf.Clamp01(baselineProbability);
            probability = Mathf.Clamp01(probability);
        }
        
        public void Transition(float val)
        {
            flowMeter = val;
            history.Add(val);
            historyProbability.Add(probability);

            if (receiver is null)
            {
                return;
            }
            receiver.Receive(val);
        }

        public void AdjustProbability(float updatedProb, bool adjustBaseline = false)
        {
            if (adjustBaseline)
            {
                baselineProbability = updatedProb;
            }

            probability = updatedProb;
        }

        public void RegisterSender(ISender newSender)
        {
            sender = newSender;
        }

        public void RegisterReceiver(IReceiver newReceiver)
        {
            receiver = newReceiver;
        }

        public float GetReading()
        {
            return flowMeter;
        }
    }
}