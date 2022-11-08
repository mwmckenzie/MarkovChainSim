// Markov Chain Sim -- AdjustmentFunction.cs
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
using UnityEngine;

namespace MarkovChainModel
{
    public abstract class AdjustmentFunction : MonoBehaviour
    {
        protected float _currVal;

        protected float _observation;
        // protected float _targetDiffTolerance = .1f;

        public float stepDuration = 1f;
        public List<float> history;


        private void Awake()
        {
            ResetHistory();
        }

        public virtual void ResetHistory()
        {
            history = new List<float>();
            _currVal = 0f;
        }

        public void SetPos(float pos)
        {
            _currVal = pos;
        }

        // public virtual void TryUpdateObservation(float submittedObs) {
        //     var diff = Mathf.Abs(_observation - submittedObs);
        //     if (diff < _targetDiffTolerance) {
        //         return;
        //     }
        //     _observation = submittedObs;
        //     OnObservationChanged();
        // }

        // protected virtual void OnObservationChanged() {
        //     
        // }

        public virtual float Adjust(float observation)
        {
            return observation;
        }
    }
}