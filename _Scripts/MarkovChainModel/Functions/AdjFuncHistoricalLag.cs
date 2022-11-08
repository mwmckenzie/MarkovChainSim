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

namespace MarkovChainModel
{
    [Serializable]
    public class AdjFuncHistoricalLag : AdjustmentFunction
    {
        [Title("Config Settings")] [InlineEditor] [SerializeField]
        private AdjFuncConfig _config;

        [InlineEditor] [SerializeField] private SecondOrderFunctionConfig _dynamicsConfig;

        [Title("Debug Lists")] public List<float> obsHistory;
        public List<float> dynamicValHistory;

        private SecondOrderDynamicsFloat _dynamics;

        public override void ResetHistory()
        {
            _config.remappedValue = 0f;

            obsHistory = new List<float>();
            dynamicValHistory = new List<float>();
            base.ResetHistory();
        }

        public override float Adjust(float observation)
        {
            if (_dynamics == null)
            {
                _dynamics = new SecondOrderDynamicsFloat(
                    _dynamicsConfig.freq, _dynamicsConfig.damp, _dynamicsConfig.response, _currVal);
            }

            _currVal = _dynamics.Update(stepDuration, observation);

            var alpha = Mathf.InverseLerp(_config.expectedObsRange.x, _config.expectedObsRange.y, _currVal);
            _config.remappedValue = Mathf.Lerp(_config.remapRange.x, _config.remapRange.y, alpha);

            history.Add(_config.remappedValue);

            obsHistory.Add(observation);
            dynamicValHistory.Add(_currVal);

            return _config.remappedValue;
        }
    }
}