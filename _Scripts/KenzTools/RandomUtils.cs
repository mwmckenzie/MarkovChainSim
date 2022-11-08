// Markov Chain Sim -- RandomUtils.cs
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
    public static class RandomUtils
    {
        public static float GetRandomValUsingCurve(AnimationCurve curve)
        {
            var candidateFound = false;
            var candidateVal = 0f;

            while (!candidateFound)
            {
                candidateVal = Random.value;
                var chanceVal = Random.value;
                candidateFound = chanceVal <= curve.Evaluate(candidateVal);
            }

            return candidateVal;
        }

        public static float CalculateTransitionCount(float pool, float transProb)
        {
            if (transProb == 1f)
            {
                return pool;
            }

            var transCount = 0f;

            if (transProb > 1f)
            {
                var mult = (int)transProb;
                transCount = pool * mult;
                transProb -= mult;
            }

            var poolInt = (int)pool;
            var poolDeci = pool - poolInt;

            for (int i = 0; i < poolInt; i++)
            {
                if (Random.value < transProb)
                {
                    transCount++;
                }
            }

            if (poolDeci > 0f)
            {
                if (Random.value < transProb)
                {
                    transCount += poolDeci;
                }
            }

            return transCount;

            // var val = 0f;
            // while (pool > 0f) {
            //     val = pool < 1f ? pool : 1f;
            //     pool--;
            //     
            //     if (Random.value < transProb) {
            //         transCount += val;
            //     }
            // } 

            // for (int i = 0; i < poolCount; i++) {
            //     if (Random.value < transProb) {
            //         transCount++;
            //     }
            // }
        }

        public static float CalculateFloatTrans(float poolCount, float transProb)
        {
            var transCount = 0f;
            int i = 0;
            while (true)
            {
                if (i < poolCount)
                {
                    transCount += Random.value * i;
                }
                else
                {
                    transCount += Random.value * (poolCount - (i - 1f));
                    return transCount;
                }

                i++;
            }
        }
    }
}