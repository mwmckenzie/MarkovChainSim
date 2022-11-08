using System.Collections.Generic;
using UnityEngine;

namespace MarkovChainModel {
    public abstract class AdjustmentFunction : MonoBehaviour {
        
        protected float _currVal;
        protected float _observation;
        // protected float _targetDiffTolerance = .1f;

        public float stepDuration = 1f;
        public List<float> history;
        
        
        private void Awake() {
            ResetHistory();
        }

        public virtual void ResetHistory() {
            history = new List<float>();
            _currVal = 0f;
        }
        
        public void SetPos(float pos) {
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
        
        public virtual float Adjust(float observation) {
            return observation;
        }
    }
    
}