using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Source : MarkovModule, ISender, ISensible {

        [Title("Event Broadcast Channels")][Required]
        public VoidEventChannelSO sendPoolBroadcast;

        [Title("Outflow")]
        public List<Connector> connectorsOut;
        
        [Title("Values")]
        public float pool;
        
        
        private void Start() {
            sendPoolBroadcast.voidEvent += OnSendPoolBroadcast;
            //cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            
            foreach (var connector in connectorsOut) {
                connector.RegisterSender(this);
            }
        }

        protected override void OnValidate() {
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast() {
            throw new NotImplementedException();
        }


        private void OnSendPoolBroadcast() {
            Send();
        }

        public void Send() {
            foreach (var connector in connectorsOut) {
                connector.Transition(pool);
            }
            history.Add(pool);
        }

        public float GetReading() {
            return pool;
        }
    }
}