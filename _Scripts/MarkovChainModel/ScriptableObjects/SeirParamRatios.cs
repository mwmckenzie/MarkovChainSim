// Markov Chain Sim -- SeirParamRatios.cs
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
    [CreateAssetMenu(fileName = "SeirParamRatios_", menuName = "Scriptable Object/Probabilities/SEIR Ratios",
        order = 0)]
    public class SeirParamRatios : ScriptableObject
    {
        [BoxGroup("Ratio Connectors")]
        [TitleGroup("Ratio Connectors/Hospitalized To Non-Hospitalized Ratio"), HideLabel]
        [InlineEditor]
        public ParameterProportion _hospToNonHospProportion;

        [TitleGroup("Ratio Connectors/Asymptomatic To Symptomatic Ratio"), HideLabel] [InlineEditor]
        public ParameterProportion _asympToSympProportion;

        [TitleGroup("Ratio Connectors/Critical To Hospitalized Ratio"), HideLabel] [InlineEditor]
        public ParameterProportion _criticalToHospProportion;

        [TitleGroup("Ratio Connectors/Deceased To Critical Recovered Ratio"), HideLabel] [InlineEditor]
        public ParameterProportion _deceasedToCriticalRecovProportion;
    }
}