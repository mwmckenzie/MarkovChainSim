using UnityEngine;

namespace KenzTools {
    public class CalcUtils {
        
        public static float ClampedWeightedCombination(float val1, float val2, float val1Val2WeightRatio) {
            var clampedV1 = Mathf.Clamp(val1, -1f, 1f);
            var clampedV2 = Mathf.Clamp(val2, -1f, 1f);
            var clampedRatio = Mathf.Clamp01(val1Val2WeightRatio);
            
            clampedV1 *= clampedRatio;
            clampedV2 *= (1 - clampedRatio);

            return Mathf.Clamp(clampedV1 + clampedV2, -1f, 1f);
        }
        
        public static float ClampedCombination(float val1, float val2) {
            var clampedV1 = Mathf.Clamp(val1, -1f, 1f);
            var clampedV2 = Mathf.Clamp(val2, -1f, 1f);
            
            return Mathf.Clamp(clampedV1 + clampedV2, -1f, 1f);
        }
    }
}