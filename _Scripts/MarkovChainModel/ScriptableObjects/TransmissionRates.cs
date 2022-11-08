using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "TransmissionRates_", menuName = "Scriptable Object/Probabilities/Transmission", order = 0)]
    public class TransmissionRates : ScriptableObject {
        
        [FoldoutGroup("Transmission Rates")]
        [TitleGroup("Transmission Rates/Symptomatic Transmission Rate"), HideLabel][InlineEditor]
        public TransmissionProbability sympTransmissionRate;
        
        [TitleGroup("Transmission Rates/Asymptomatic Transmission Rate"), HideLabel][InlineEditor]
        public TransmissionProbability asympTransmissionRate;

        [TitleGroup("Transmission Rates/Hospitalized Transmission Rate"), HideLabel][InlineEditor]
        public TransmissionProbability hospitalTransmissionRate;
        
        [TitleGroup("Transmission Rates/Critical Transmission Rate"), HideLabel][InlineEditor]
        public TransmissionProbability criticalTransmissionRate;
        
        [TitleGroup("Transmission Rates/Visitor Transmission Rate"), HideLabel][InlineEditor]
        public TransmissionProbability visitorTransmissionRate;
    }
}