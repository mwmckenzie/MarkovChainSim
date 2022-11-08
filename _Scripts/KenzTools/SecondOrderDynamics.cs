// Concept, Math, and Scripts are from: https://www.youtube.com/watch?v=KPoeNZZ6H4s&t=312s
// todo: Rewrite for longevity in own coding style/format (keeping reference above)


using UnityEngine;
using static Unity.Mathematics.math;
using static UnityEngine.Mathf;

namespace KenzTools {
    public class SecondOrderDynamics {

        private Vector2 _xp;
        private Vector2 _y;
        private Vector2 _yd;
        private float _w;
        private float _z;
        private float _d;
        private float _k1;
        private float _k2;
        private float _k3;

        public SecondOrderDynamics(float f, float z, float r, Vector2 x0) {
            _w = 2f* Mathf.PI * f;
            _z = z;
            _d = _w * Sqrt(Abs(z * z - 1f));

            _k1 = z / (Mathf.PI * f);
            _k2 = 1f / (_w * _w);
            _k3 = r * z / _w;

            _xp = x0;
            _y = x0;
            _yd = Vector2.zero;
        }

        public Vector2 Update(float timeStep, Vector2 x, Vector2 xd, bool ignoreXdInput = false) {
            if (ignoreXdInput) {
                xd = (x - _xp) / timeStep;
                _xp = x;
            }

            var k1_stable = _k1;
            var k2Stable = 
                Max(_k2, (timeStep * timeStep / 2f) + (timeStep * _k1 / 2f), timeStep * _k1);

            if (_w * timeStep >= _z) {
                var t1 = Exp(-_z * _w * timeStep);
                var alpha = 
                    2f * t1 * (_z <= 1f ? 
                        Cos(timeStep * _d) : 
                        cosh(timeStep * _d));

                var beta = t1 * t1;
                var t2 = timeStep / (1f + beta - alpha);
                k1_stable = (1f - beta) * t2;
                k2Stable = timeStep * t2;
            }

            _y = _y + timeStep * _yd;
            _yd = _yd + timeStep * (x + _k3 * xd - _y - _k1 * _yd) / k2Stable;

            return _y;
        }
    }
}