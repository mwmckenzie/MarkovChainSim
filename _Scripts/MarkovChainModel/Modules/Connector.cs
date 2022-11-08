using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MarkovChainModel {
    public class Connector : MarkovModule, ISensible {

        [Title("Connections")]
        public ISender  sender;
        public IReceiver  receiver;
        
        [Title("Probabilities")]
        public float baselineProbability = 1f;
        public float probability = 1f;

        [Title("Clamped Flow")] 
        public bool clamped = true;
        
        [Title("Values")]
        public float flowMeter;
        
        [Title("Probability History")]
        public List<float> historyProbability;

        protected override void OnValidate() {
            
            if (TrySetSenderFromParent()) {
                SetNameFromConnections();
                var senderModule = (MarkovModule) sender;
                cleanUpBroadcast = senderModule.cleanUpBroadcast;
            }
            else {
                SetNameIfNothing();
            }
            
            ClampProbabilities();
        }

        private bool TrySetSenderFromParent() {
            var module = GetComponentInParent<ISender>();
            if (module is null) {
                return false;
            }

            sender = module;
            return true;
        }

        private void SetNameFromConnections() {
            var senderModule = (MarkovModule) sender;
            var front = senderModule.name;
            var back = "";
            if (receiver != null) {
                var receiverModule = (MarkovModule) receiver;
                back = receiverModule.name;
            }
            name = $"{front}To{back}";
            gameObject.name = name;
        }
        
        private void Start() {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            ResetProbabilities();
            if (clamped) {
                ClampProbabilities();
            }
        }

        private void OnCleanUpBroadcast() {
            ResetProbabilities();
            if (clamped) {
                ClampProbabilities();
            }
        }
        
        private void SetNameIfNothing() {
            if (name.IsNullOrWhitespace()) {
                name = gameObject.name;
            }
        }

        private void ResetProbabilities() {
            probability = baselineProbability;
        }
        
        private void ClampProbabilities() {
            baselineProbability = Mathf.Clamp01(baselineProbability);
            probability = Mathf.Clamp01(probability);
        }

        // public void Transition(int val) {
        //     flowMeter = val;
        //     history.Add(val);
        //     historyProbability.Add(probability);
        //     
        //     if (receiver is null) { return; }
        //     
        //     receiver.Receive(val);
        // }

        public void Transition(float val) {
            flowMeter = val;
            history.Add(val);
            historyProbability.Add(probability);
            
            if (receiver is null) { return; }
            
            receiver.Receive(val);
        }

        public void AdjustProbability(float updatedProb, bool adjustBaseline = false) {
            if (adjustBaseline) {
                baselineProbability = updatedProb;
            }
            probability = updatedProb;
        }

        public void RegisterSender(ISender newSender) {
            sender = newSender;
        }

        public void RegisterReciever(IReceiver newReceiver) {
            receiver = newReceiver;
        }

        public float GetReading() {
            return flowMeter;
        }
    }
}