using UnityEngine;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "AdjFuncConfig_", 
        menuName = "Scriptable Object/Configuration/Adjustment Function", 
        order = 0)]
    public class AdjFuncConfig : ScriptableObject {
        
        public Vector2 expectedObsRange;
        public Vector2 remapRange;
        public float remappedValue;
        
    }
}