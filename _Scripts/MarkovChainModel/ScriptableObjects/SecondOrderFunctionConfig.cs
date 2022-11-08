using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "SecondOrderFunctionConfig_", menuName = "Scriptable Object/Configuration/Second Order Function", order = 0)]
    public class SecondOrderFunctionConfig : ScriptableObject {
        
        public float freq = 1f;
        public float damp = 0.5f;
        public float response = 2f;
    }
}