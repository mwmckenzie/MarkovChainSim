// Markov Chain Sim -- CalcUtils.cs
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

namespace KenzTools
{
    public class CalcUtils
    {
        public static float ClampedWeightedCombination(float val1, float val2, float val1Val2WeightRatio)
        {
            var clampedV1 = Mathf.Clamp(val1, -1f, 1f);
            var clampedV2 = Mathf.Clamp(val2, -1f, 1f);
            var clampedRatio = Mathf.Clamp01(val1Val2WeightRatio);

            clampedV1 *= clampedRatio;
            clampedV2 *= (1 - clampedRatio);

            return Mathf.Clamp(clampedV1 + clampedV2, -1f, 1f);
        }

        public static float ClampedCombination(float val1, float val2)
        {
            var clampedV1 = Mathf.Clamp(val1, -1f, 1f);
            var clampedV2 = Mathf.Clamp(val2, -1f, 1f);

            return Mathf.Clamp(clampedV1 + clampedV2, -1f, 1f);
        }
    }
}