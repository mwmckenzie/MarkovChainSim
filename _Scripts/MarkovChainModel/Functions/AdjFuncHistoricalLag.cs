// Markov Chain Sim -- AdjFuncHistoricalLag.cs
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
using System.Collections.Generic;
using KenzTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MarkovChainModel
{
    [Serializable]
    public class AdjFuncHistoricalLag : AdjustmentFunction
    {
        [FormerlySerializedAs("_config")]
        [Title("Config Settings")]
        [SerializeField] [InlineEditor] 
        private AdjFuncConfig config;

        [FormerlySerializedAs("_dynamicsConfig")]
        [InlineEditor] 
        [SerializeField] 
        private SecondOrderFunctionConfig dynamicsConfig;

        [Title("Debug Lists")] 
        public List<float> obsHistory;
        public List<float> dynamicValHistory;

        private SecondOrderDynamicsFloat _dynamics;

        public override void ResetHistory()
        {
            config.remappedValue = 0f;

            obsHistory = new List<float>();
            dynamicValHistory = new List<float>();
            base.ResetHistory();
        }

        public override float Adjust(float obsIn)
        {
            _dynamics ??= new SecondOrderDynamicsFloat(
                dynamicsConfig.freq, dynamicsConfig.damp, dynamicsConfig.response, currVal);

            currVal = _dynamics.Update(stepDuration, obsIn);

            var alpha = Mathf.InverseLerp(config.expectedObsRange.x, 
                config.expectedObsRange.y, currVal);
            
            config.remappedValue = Mathf.Lerp(config.remapRange.x, config.remapRange.y, alpha);

            history.Add(config.remappedValue);

            obsHistory.Add(obsIn);
            dynamicValHistory.Add(currVal);

            return config.remappedValue;
        }
    }
}