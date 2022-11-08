using System.Collections.Generic;
using System.Linq;
using KenzTools;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Splitter : MarkovModule, IReceiver, ISender {

        [Title("Flow In")]
        public List<Connector> connectorsIn;
        
        [Title("Split Outflow")]
        public List<Connector> connectorsOut;

        [Title("Values")]
        public float pool;
        public float incoming;

        private void Start() {
            foreach (var connector in connectorsOut) {
                connector.RegisterSender(this);
            }
            RegisterAsReceiver();
        }
        
        protected override void OnValidate() {
            RegisterAsReceiver();
            UpdateOutputsFromChildren();
            OrderOutputsByProbability();
            base.OnValidate();
        }

        private void UpdateOutputsFromChildren() {
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
        }

        private void OnSendSplitsBroadcast() {
            Send();
        }
        
        public void Receive(float val) {
            pool += val;
            Send();
        }

        public void RegisterAsReceiver() {
            foreach (var connector in connectorsIn) {
                connector.RegisterReciever(this);
            }
        }

        private void OrderOutputsByProbability() {
            connectorsOut = connectorsOut.OrderBy(x => x.probability).ToList();
        }

        public void Send() {
            OrderOutputsByProbability();
            
            foreach (var connector in connectorsOut) {
                if (pool > 0) {
                    var output = RandomUtils.CalculateTransitionCount(pool, connector.probability);
                    connector.Transition(output);
                    pool -= output;
                }
                else {
                    connector.Transition(0);
                }
            }
            pool = 0;
        }
    }
}