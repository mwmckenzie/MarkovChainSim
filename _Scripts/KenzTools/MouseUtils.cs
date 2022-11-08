using UnityEngine;

namespace KenzTools {
    public static class MouseUtils {
        
        public static Vector3 MouseWorldPoint() {
            return Camera.main == null ? Vector3.zero : Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        
    }
}