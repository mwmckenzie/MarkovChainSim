// Markov Chain Sim -- ParameterProportion.cs
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
    [CreateAssetMenu(fileName = "ParamProportion_", menuName = "Scriptable Object/Probability/Parameter Proportion",
        order = 0)]
    public class ParameterProportion : ScriptableObject
    {
        [Title("Calculation")] [OnValueChanged("OnAsRatioChange"), PropertyOrder(-1)]
        public bool asRatio;

        [OnValueChanged("OnAsProportionChange"), PropertyOrder(-1)]
        public bool asProportion;

        [Title("Parameter A"), HideLabel, PropertyOrder(1)]
        public string paramA;

        [OnValueChanged("OnValueAChange"), PropertyOrder(2)] [DelayedProperty]
        public float valueA;

        [Title("Parameter B"), HideLabel, PropertySpace, PropertyOrder(4)]
        public string paramB;

        [OnValueChanged("OnValueBChange"), PropertyOrder(5)] [DelayedProperty]
        public float valueB;

        [Title("Proportion"), HideLabel, ShowInInspector, PropertyOrder(7), PropertySpace]
        public string proportionName => $"{paramA}To{paramB}Proportion";

        [OnValueChanged("OnPerValueChange"), PropertyOrder(8)] [DelayedProperty]
        public float per;

        [OnValueChanged("OnProportionChanged"), PropertyOrder(9)] [DelayedProperty]
        public float proportion;

        private void OnAsRatioChange()
        {
            asProportion = !asRatio;
        }

        private void OnAsProportionChange()
        {
            asRatio = !asProportion;
        }

        private void OnValueAChange()
        {
            Calculate();
            CleanUp();
        }

        private void OnValueBChange()
        {
            Calculate();
            CleanUp();
        }

        private void OnPerValueChange()
        {
            Calculate(true);
            CleanUp();
        }

        private void OnProportionChanged()
        {
            if (asRatio)
            {
                per = (valueA + valueB);
            }

            valueA = proportion * per;
            valueB = per - valueA;
            CleanUp();
        }

        private void Calculate(bool perChanged = false)
        {
            if (asProportion)
            {
                proportion = per == 0f ? 0f : valueA / per;
                valueB = per - valueA;
                return;
            }

            if (!perChanged)
            {
                per = (valueA + valueB);
            }

            proportion = per == 0f ? 0f : valueA / per;
        }

        private void CleanUp()
        {
            proportion = proportion > 1f ? 1f : proportion;
            valueB = valueB < 0f ? 0f : valueB;
        }
    }
}