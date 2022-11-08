using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Sink : MarkovModule, IReceiver, ISensible {

        [Title("Inflow")]
        public List<Connector> connectorsIn;
        
        [Title("Values")]
        public float pool;

        private float lastValue;

        private void Start() {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            RegisterAsReceiver();
        }
        
        protected override void OnValidate() {
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast() {
            CleanUp();
        }

        private void CleanUp() {
            lastValue = pool;
            history.Add(pool);
            pool = 0;
        }

        public void Receive(float val) {
            pool += val;
        }

        public void RegisterAsReceiver() {
            foreach (var connector in connectorsIn) {
                connector.RegisterReciever(this);
            }
        }

        public float GetReading() {
            return lastValue;
        }
    }
}