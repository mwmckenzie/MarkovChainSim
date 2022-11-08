using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "ParamProportion_", menuName = "Scriptable Object/Probability/Parameter Proportion", order = 0)]
    public class ParameterProportion : ScriptableObject {

        [Title("Calculation")] [OnValueChanged("OnAsRatioChange"), PropertyOrder(-1)]
        public bool asRatio;
        [OnValueChanged("OnAsProportionChange"), PropertyOrder(-1)]
        public bool asProportion;

        [Title("Parameter A"), HideLabel, PropertyOrder(1)]
        public string paramA;
        [OnValueChanged("OnValueAChange"), PropertyOrder(2)][DelayedProperty]
        public float valueA;

        [Title("Parameter B"), HideLabel, PropertySpace, PropertyOrder(4)]
        public string paramB;
        [OnValueChanged("OnValueBChange"), PropertyOrder(5)][DelayedProperty]
        public float valueB;

        [Title("Proportion"), HideLabel, ShowInInspector, PropertyOrder(7), PropertySpace]
        public string proportionName => $"{paramA}To{paramB}Proportion";
        [OnValueChanged("OnPerValueChange"), PropertyOrder(8)][DelayedProperty]
        public float per;
        [OnValueChanged("OnProportionChanged"), PropertyOrder(9)][DelayedProperty]
        public float proportion;
        
        private void OnAsRatioChange() {
            asProportion = !asRatio;
        }
        private void OnAsProportionChange() {
            asRatio = !asProportion;
        }
        
        private void OnValueAChange() {
            Calculate();
            CleanUp();
        }
        private void OnValueBChange() {
            Calculate();
            CleanUp();
        }
        private void OnPerValueChange() {
            Calculate(true);
            CleanUp();
        }

        private void OnProportionChanged() {
            if (asRatio) {
                per = (valueA + valueB);
            }
            valueA = proportion * per;
            valueB = per - valueA;
            CleanUp();
        }
        
        private void Calculate(bool perChanged = false) {
            if (asProportion) {
                proportion = per == 0f ? 0f : valueA / per;
                valueB = per - valueA;
                return;
            }
            if (!perChanged) {
                per = (valueA + valueB);
            }
            proportion = per == 0f ? 0f : valueA / per;
        }

        private void CleanUp() {
            proportion = proportion > 1f ? 1f : proportion;
            valueB = valueB < 0f ? 0f : valueB;
        }
    }
    
    
    
}