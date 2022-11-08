// Markov Chain Sim -- ConversionProbabilities.cs
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
    [CreateAssetMenu(fileName = "ConversionProbs_", menuName = "Scriptable Object/Probabilities/Conversion", order = 0)]
    public class ConversionProbabilities : ScriptableObject
    {
        [BoxGroup("Conversion Rates")]
        [TitleGroup("Conversion Rates/Exposed To Infectious Rate"), HideLabel]
        [InlineEditor]
        public ConversionProbability exposedToInfectiousRate;

        [TitleGroup("Conversion Rates/Symptomatic To Recovered Rate"), HideLabel] [InlineEditor]
        public ConversionProbability symptomaticRecoveryRate;

        [TitleGroup("Conversion Rates/Asymptomatic To Recovered Rate"), HideLabel] [InlineEditor]
        public ConversionProbability asymptomaticRecoveryRate;

        [TitleGroup("Conversion Rates/Hospitalization To Recovered Rate"), HideLabel] [InlineEditor]
        public ConversionProbability hospitalizationRecoveryRate;

        [TitleGroup("Conversion Rates/Critical To Recovered Rate"), HideLabel] [InlineEditor]
        public ConversionProbability criticalRecoveryRate;

        [TitleGroup("Conversion Rates/Recovered To Susceptible Rate"), HideLabel] [InlineEditor]
        public ConversionProbability recoveredToSusceptibleRate;

        [TitleGroup("Conversion Rates/Vaccinated To Susceptible Rate"), HideLabel] [InlineEditor]
        public ConversionProbability vaccinatedToSusceptibleRate;
    }
}