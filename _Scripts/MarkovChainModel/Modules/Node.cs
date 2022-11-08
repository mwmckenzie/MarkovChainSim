using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    public class Node : MarkovModule, ISensible, ISender, IReceiver, IPumpable {

        [Title("Event Broadcast Channels")][Required]
        public VoidEventChannelSO sendPoolBroadcast;

        [Title("Inflow")]
        public List<Connector> connectorsIn;
        
        [Title("Outflow")][HideLabel]
        public Connector connectorOut;
        
        [Title("Values")]
        public float pool;
        public float incoming;

        private void Start() {
            sendPoolBroadcast.voidEvent += OnSendPoolBroadcast;
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;

            if (connectorOut != null) {
                connectorOut.RegisterSender(this);
            }
            RegisterAsReceiver();
        }

        protected override void OnValidate() {
            connectorOut = GetComponentInChildren<Connector>();
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast() {
            pool = incoming;
            incoming = 0;
            history.Add(pool);
        }

        private void OnSendPoolBroadcast() {
            Send();
        }

        public void Receive(float val) {
            incoming += val;
        }

        public void RegisterAsReceiver() {
            foreach (var connector in connectorsIn) {
                connector.RegisterReciever(this);
            }
        }

        public void Send() {
            if (connectorOut is null) {
                incoming += pool;
                return;
            }
            connectorOut.Transition(pool);
            pool = 0;
        }

        public float GetReading() {
            return pool;
        }

        public float Pump(float val) {
            var amount = Mathf.Min(val, pool);
            pool -= amount;
            return amount;
        }
        
    }
}