// Markov Chain Sim -- ConversionProbability.cs
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

using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel
{
    [CreateAssetMenu(fileName = "ConversionProb_", menuName = "Scriptable Object/Probability/Conversion", order = 0)]
    public class ConversionProbability : ScriptableObject
    {
        [Title("Conversion")] [HideLabel] public string conversionName;

        [HideLabel] [OnValueChanged("OnConversionSliderValueChange")] [ProgressBar(0.0, 1.0, .4f, .8f, 1f, Height = 20)]
        public float conversionSlider;

        [OnValueChanged("OnRateValueChange")] [MinValue(0f)] [MaxValue(1f)] [DelayedProperty]
        public float conversionRate;

        [OnValueChanged("OnPeriodValueChange")] [MinValue(1f)] [DelayedProperty]
        public float avgPeriod;


        private void OnPeriodValueChange()
        {
            conversionRate = 1 / avgPeriod;
            conversionSlider = EaseOutExpo(conversionRate);
            SetName();
        }

        private void OnRateValueChange()
        {
            conversionSlider = EaseOutExpo(conversionRate);
            avgPeriod = 1 / conversionRate;
            SetName();
        }

        private void OnConversionSliderValueChange()
        {
            conversionRate = EaseInExpo(conversionSlider);
            avgPeriod = 1 / conversionRate;
            SetName();
        }

        private void SetName()
        {
            conversionName = name;
        }

        private float EaseInExpo(float number)
        {
            return number == 0f ? Mathf.Epsilon : Mathf.Pow(2f, 10f * number - 10f);
        }

        private float EaseOutExpo(float number)
        {
            return number == 1f ? 1f : (Mathf.Log(number, 2) + 10f) / 10f;
        }
    }
}