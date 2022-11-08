// Markov Chain Sim -- TransmissionProbability.cs
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
    [CreateAssetMenu(fileName = "TransmissionProb_", menuName = "Scriptable Object/Probability/Transmission",
        order = 0)]
    public class TransmissionProbability : ScriptableObject
    {
        [Title("Transmission Rate"), ShowInInspector, PropertyOrder(0), PropertySpace]
        public string referenceName;

        [HideLabel]
        [OnValueChanged("OnTransmissionSliderValueChange"), PropertyOrder(1)]
        [ProgressBar(0.0, 1.0, .4f, .8f, 1f, Height = 20), PropertySpace]
        public float transmissionSlider;

        [OnValueChanged("OnRateValueChange"), PropertyOrder(2)] [DelayedProperty]
        public float rate;

        [OnValueChanged("OnSusceptibilityValueChange")]
        [MinValue(0f)]
        [DelayedProperty]
        [Title("Transmission Variables"), PropertyOrder(3), PropertySpace]
        public float susceptibility;

        [OnValueChanged("OnCountValueChange")] [MinValue(1f)] [DelayedProperty, PropertyOrder(4)]
        public float avgCountPerPeriod = 1f;

        [OnValueChanged("OnPeriodValueChange")] [MinValue(1f)] [DelayedProperty, PropertyOrder(5)]
        public float avgPeriod;


        private void OnSusceptibilityValueChange()
        {
            // avgCountPerPeriod *= susceptibility;
            // rate = avgCountPerPeriod / avgPeriod;
            rate = avgCountPerPeriod * susceptibility / avgPeriod;
            transmissionSlider = EaseOutExpo(rate);
        }

        private void OnCountValueChange()
        {
            // susceptibility *= avgCountPerPeriod;
            // rate = avgCountPerPeriod / avgPeriod;
            rate = avgCountPerPeriod * susceptibility / avgPeriod;
            transmissionSlider = EaseOutExpo(rate);
        }

        private void OnPeriodValueChange()
        {
            rate = avgCountPerPeriod * susceptibility / avgPeriod;
            transmissionSlider = EaseOutExpo(rate);
        }

        private void OnRateValueChange()
        {
            transmissionSlider = EaseOutExpo(rate);
            avgPeriod = avgCountPerPeriod * susceptibility / avgPeriod;
        }

        private void OnTransmissionSliderValueChange()
        {
            rate = EaseInExpo(transmissionSlider);
            avgPeriod = avgCountPerPeriod * susceptibility / avgPeriod;
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