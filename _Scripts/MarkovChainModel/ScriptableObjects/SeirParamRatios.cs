using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "SeirParamRatios_", menuName = "Scriptable Object/Probabilities/SEIR Ratios", order = 0)]
    public class SeirParamRatios : ScriptableObject {
        
        [BoxGroup("Ratio Connectors")]
        [TitleGroup("Ratio Connectors/Hospitalized To Non-Hospitalized Ratio"), HideLabel][InlineEditor]
        public ParameterProportion _hospToNonHospProportion;
        
        [TitleGroup("Ratio Connectors/Asymptomatic To Symptomatic Ratio"), HideLabel][InlineEditor]
        public ParameterProportion _asympToSympProportion;

        [TitleGroup("Ratio Connectors/Critical To Hospitalized Ratio"), HideLabel][InlineEditor]
        public ParameterProportion _criticalToHospProportion;
        
        [TitleGroup("Ratio Connectors/Deceased To Critical Recovered Ratio"), HideLabel][InlineEditor]
        public ParameterProportion _deceasedToCriticalRecovProportion;
    }
}