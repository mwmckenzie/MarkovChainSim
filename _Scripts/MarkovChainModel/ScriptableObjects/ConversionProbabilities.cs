using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "ConversionProbs_", menuName = "Scriptable Object/Probabilities/Conversion", order = 0)]
    public class ConversionProbabilities : ScriptableObject {
        
        [BoxGroup("Conversion Rates")]
        [TitleGroup("Conversion Rates/Exposed To Infectious Rate"), HideLabel][InlineEditor]
        public ConversionProbability exposedToInfectiousRate;

        [TitleGroup("Conversion Rates/Symptomatic To Recovered Rate"), HideLabel][InlineEditor]
        public ConversionProbability symptomaticRecoveryRate;

        [TitleGroup("Conversion Rates/Asymptomatic To Recovered Rate"), HideLabel][InlineEditor]
        public ConversionProbability asymptomaticRecoveryRate;

        [TitleGroup("Conversion Rates/Hospitalization To Recovered Rate"), HideLabel][InlineEditor]
        public ConversionProbability hospitalizationRecoveryRate;

        [TitleGroup("Conversion Rates/Critical To Recovered Rate"), HideLabel][InlineEditor]
        public ConversionProbability criticalRecoveryRate;

        [TitleGroup("Conversion Rates/Recovered To Susceptible Rate"), HideLabel][InlineEditor]
        public ConversionProbability recoveredToSusceptibleRate;

        [TitleGroup("Conversion Rates/Vaccinated To Susceptible Rate"), HideLabel][InlineEditor]
        public ConversionProbability vaccinatedToSusceptibleRate;
        
    }
}