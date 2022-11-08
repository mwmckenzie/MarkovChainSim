// Markov Chain Sim -- SecondOrderDynamicsFloat.cs
// 
// Copyright (C) 2022 Matthew W. McKenzie and Kenz LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

// todo: Rewrite for longevity in own coding style/format (keeping reference above)


using UnityEngine;
using static Unity.Mathematics.math;
using static UnityEngine.Mathf;

namespace KenzTools
{
    public class SecondOrderDynamicsFloat
    {
        private float _xPrior;
        private float _y;
        private float _yDelta;
        private float _w;
        private float _z;
        private float _d;
        private float _k1;
        private float _k2;
        private float _k3;

        public SecondOrderDynamicsFloat(float f, float z, float r, float x0)
        {
            _w = 2f * Mathf.PI * f;
            _z = z;
            _d = _w * Sqrt(Abs(z * z - 1f));

            _k1 = z / (Mathf.PI * f);
            _k2 = 1f / (_w * _w);
            _k3 = r * z / _w;

            _xPrior = x0;
            _y = x0;
            _yDelta = 0f;
        }

        public float Update(float timeStep, float x, float xDelta = 0f, bool xdUnknown = true)
        {
            if (xdUnknown)
            {
                xDelta = (x - _xPrior) / timeStep;
                _xPrior = x;
            }

            var k1_stable = _k1;
            var k2Stable =
                Max(_k2, (timeStep * timeStep / 2f) + (timeStep * _k1 / 2f), timeStep * _k1);

            if (_w * timeStep >= _z)
            {
                var t1 = Exp(-_z * _w * timeStep);
                var alpha =
                    2f * t1 * (_z <= 1f ? Cos(timeStep * _d) : cosh(timeStep * _d));

                var beta = t1 * t1;
                var t2 = timeStep / (1f + beta - alpha);
                k1_stable = (1f - beta) * t2;
                k2Stable = timeStep * t2;
            }

            _y = _y + timeStep * _yDelta;
            _yDelta = _yDelta + timeStep * (x + _k3 * xDelta - _y - _k1 * _yDelta) / k2Stable;

            return _y;
        }
    }
}