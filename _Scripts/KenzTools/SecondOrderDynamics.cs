// Markov Chain Sim -- SecondOrderDynamics.cs
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

using UnityEngine;
using static Unity.Mathematics.math;
using static UnityEngine.Mathf;

namespace KenzTools
{
    public class SecondOrderDynamics
    {
        private Vector2 _xPrior;
        private Vector2 _y;
        private Vector2 _yDelta;
        private readonly float _w;
        private readonly float _z;
        private readonly float _d;
        private readonly float _k1;
        private readonly float _k2;
        private readonly float _k3;

        public SecondOrderDynamics(float f, float z, float r, Vector2 xStart)
        {
            _w = 2f * Mathf.PI * f;
            _z = z;
            _d = _w * Sqrt(Abs(z * z - 1f));

            _k1 = z / (Mathf.PI * f);
            _k2 = 1f / (_w * _w);
            _k3 = r * z / _w;

            _xPrior = xStart;
            _y = xStart;
            _yDelta = Vector2.zero;
        }

        public Vector2 Update(float timeStep, Vector2 x, Vector2 xDelta, bool ignoreXdInput = false)
        {
            if (ignoreXdInput)
            {
                xDelta = (x - _xPrior) / timeStep;
                _xPrior = x;
            }

            var k1Stable = _k1;
            var k2Stable =
                Max(_k2, (timeStep * timeStep / 2f) + (timeStep * _k1 / 2f), timeStep * _k1);

            if (_w * timeStep >= _z)
            {
                var t1 = Exp(-_z * _w * timeStep);
                var alpha =
                    2f * t1 * (_z <= 1f ? Cos(timeStep * _d) : cosh(timeStep * _d));

                var beta = t1 * t1;
                var t2 = timeStep / (1f + beta - alpha);
                k1Stable = (1f - beta) * t2;
                k2Stable = timeStep * t2;
            }

            _y = _y + timeStep * _yDelta;
            _yDelta = _yDelta + timeStep * (x + _k3 * xDelta - _y - _k1 * _yDelta) / k2Stable;

            return _y;
        }
    }
}