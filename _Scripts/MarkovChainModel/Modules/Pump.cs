using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Pump : MarkovModule, IReceiver, ISender {

        [Title("Pump Source Node")][HideLabel][Required]
        public IPumpable pumpSource;
        
        [Title("Signal In")][HideLabel][Required]
        public Connector connectorIn;
        
        [Title("Pump Outflow")][HideLabel][Required]
        public Connector connectorOut;
        
        [Title("Values")]
        public float pool;
        public float signal;
        

        private void Start() {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            
            connectorOut.RegisterSender(this);
            RegisterAsReceiver();
        }

        protected override void OnValidate() {
            connectorOut = GetComponentInChildren<Connector>();
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast() {
        }
        
        
        public void Receive(float val) {
            signal = val;
            PumpFromSource();
        }

        public void RegisterAsReceiver() {
            connectorIn.RegisterReciever(this);
        }

        public void PumpFromSource() {
            pool = pumpSource.Pump(signal);
            Send();
        }

        public void Send() {
            connectorOut.Transition(pool);
            pool = 0;
        }
    }
}