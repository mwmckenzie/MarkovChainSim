using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Sensor : MarkovModule, ISender {

        [Title("Event Broadcast Channels")][Required]
        public VoidEventChannelSO takeReadingBroadcast;
        [Required]
        public VoidEventChannelSO sendReadingBroadcast;

        [Title("Monitored Object")][HideLabel][Required]
        public ISensible monitoredObj;
        
        [Title("Signal(s) Out")]
        public List<Connector> connectorsOut;
        
        [Title("Values")]
        public float reading;

        private void Start() {
            takeReadingBroadcast.voidEvent += OnTakeReadingBroadcast;
            sendReadingBroadcast.voidEvent += OnSendReadingBroadcast;
            foreach (var connector in connectorsOut) {
                connector.RegisterSender(this);
            }
        }

        protected override void OnValidate() {
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
            base.OnValidate();
        }

        private void OnSendReadingBroadcast() {
            Send();
        }

        private void OnTakeReadingBroadcast() {
            reading = monitoredObj.GetReading();
            history.Add(reading);
            
        }

        public void Send() {
            foreach (var connector in connectorsOut) {
                connector.Transition(reading);
            }
        }
    }
}