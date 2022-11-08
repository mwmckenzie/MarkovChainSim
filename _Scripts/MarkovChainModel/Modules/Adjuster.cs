using System;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class Adjuster : MarkovModule, IReceiver {

        [Title("Connector to Adjust")][HideLabel][Required]
        public Connector adjustableConnector;
        public bool adjustBaselineProbability;
        
        [Title("Signal In")][HideLabel][Required]
        public Connector connectorIn;

        [Title("Adjustment Type")] [HideLabel]
        public AdjustmentType adjustmentType;
        
        [Title("Adjustment Function")] [HideLabel, InlineEditor] [Required]
        public AdjustmentFunction adjustmentFunction;
        
        [Title("Values")]
        public float signal;
        public float adjustedProbability;
        
        private void Start() {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            RegisterAsReceiver();
            adjustmentFunction.ResetHistory();
        }
        
        protected override void OnValidate() {
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast() {
            history.Add(adjustedProbability);
            signal = 0f;
        }

        private void Adjust(float val) {
            signal = val;
            adjustedProbability = adjustmentFunction.Adjust(signal);
            //adjustableConnector.probability = adjustedProbability;

            var priorProb = adjustableConnector.probability;
            
            switch (adjustmentType) {
                case AdjustmentType.Replace:
                    break;
                case AdjustmentType.Sum:
                    adjustedProbability += priorProb;
                    break;
                case AdjustmentType.Multiply:
                    adjustedProbability *= priorProb;
                    break;
                case AdjustmentType.Divide:
                    if (adjustedProbability != 0f) {
                        adjustedProbability = priorProb / adjustedProbability;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // adjustableConnector.probability = adjustedProbability;
            // if (adjustBaselineProbability) {
            //     adjustableConnector.baselineProbability = adjustedProbability;
            // }
            
            adjustableConnector.AdjustProbability(adjustedProbability, adjustBaselineProbability);
        }

        public void Receive(float val) {
            Adjust(val);
        }

        public void RegisterAsReceiver() {
            connectorIn.RegisterReciever(this);
        }
    }

    public enum AdjustmentType {
        Replace,
        Sum,
        Multiply,
        Divide
    }
}