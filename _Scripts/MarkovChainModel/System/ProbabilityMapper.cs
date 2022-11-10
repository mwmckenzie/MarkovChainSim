// Markov Chain Sim -- ProbabilityMapper.cs
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

using System;
using KenzTools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel
{
    public class ProbabilityMapper : MonoBehaviour
    {
        [Title("Current Probabilities & Ratios")] [PropertyOrder(-2)]
        public string referenceName;

        [SerializeField] private SeirModelParameters _modelParameters;

        [HorizontalGroup("ParamsExportSplit", .8f)]
        [Button("Export Params [JSON]", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(-2), PropertySpace(10, 10)]
        private void ExportParametersButton()
        {
            if (_modelParameters is null)
                return;
            FileExportUtils.ExportJson(_modelParameters, "SeirModelParams", referenceName);
        }

        [SerializeField]
        [Title("Conversion Probabilities")] [HideLabel, PropertyOrder(-1)] 
        private ConversionProbabilities _conversionProbs;

        [SerializeField]
        [Title("Parameter Ratios")] [HideLabel, PropertyOrder(-1)]
        private SeirParamRatios _paramRatios;

        [SerializeField]
        [Title("Transmission Rates")] [HideLabel, PropertyOrder(-1)] 
        private TransmissionRates _transmissionRates;

        [HorizontalGroup("ModelRefreshSplit", .8f)]
        [Button("Refresh Probabilities", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(-1), PropertySpace(10, 10)]
        private void RefreshModelProbabilitiesButton()
        {
            MapProbabilities();
        }

        [FoldoutGroup("Conversion Connectors")]
        [TitleGroup("Conversion Connectors/Exposed To Infectious Connector"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector exposedToInfectiousRate;

        [TitleGroup("Conversion Connectors/Symptomatic To Recovered Connector"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector symptomaticRecoveryRate;

        [TitleGroup("Conversion Connectors/Asymptomatic To Recovered Connector"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector asymptomaticRecoveryRate;

        [TitleGroup("Conversion Connectors/Hospitalization To Recovered Connector"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector hospitalizationRecoveryRate;

        [TitleGroup("Conversion Connectors/Critical To Recovered Connector"), HideLabel] [InlineEditor] [SerializeField]
        private Connector criticalRecoveryRate;

        [TitleGroup("Conversion Connectors/Recovered To Susceptible Connector"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector recoveredToSusceptibleRate;

        [TitleGroup("Conversion Connectors/Vaccinated To Susceptible Rate"), HideLabel] [InlineEditor] [SerializeField]
        private Connector vaccinatedToSusceptibleRate;

        [FoldoutGroup("Ratio Connectors")]
        [TitleGroup("Ratio Connectors/Hospitalized To Non-Hospitalized Ratio"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector hospToNonHospRatio;

        [TitleGroup("Ratio Connectors/Asymptomatic To Symptomatic Ratio"), HideLabel] [InlineEditor] [SerializeField]
        private Connector asympToSympRatio;

        [TitleGroup("Ratio Connectors/Critical To Hospitalized Ratio"), HideLabel] [InlineEditor] [SerializeField]
        private Connector criticalToHospRatio;

        [TitleGroup("Ratio Connectors/Deceased To Critical Recovered Ratio"), HideLabel] [InlineEditor] [SerializeField]
        private Connector deceasedToCriticalRecovRatio;

        [FoldoutGroup("Transmission Rates")]
        [TitleGroup("Transmission Rates/Symptomatic Transmission Rate"), HideLabel]
        [InlineEditor]
        [SerializeField]
        private Connector sympTransmissionRate;

        [TitleGroup("Transmission Rates/Asymptomatic Transmission Rate"), HideLabel] [InlineEditor] [SerializeField]
        private Connector asympTransmissionRate;

        [TitleGroup("Transmission Rates/Hospitalized Transmission Rate"), HideLabel] [InlineEditor] [SerializeField]
        private Connector hospitalTransmissionRate;

        [TitleGroup("Transmission Rates/Critical Transmission Rate"), HideLabel] [InlineEditor] [SerializeField]
        private Connector criticalTransmissionRate;

        [TitleGroup("Transmission Rates/Visitor Transmission Rate"), HideLabel] [InlineEditor] [SerializeField]
        private Connector visitorTransmissionRate;


        private void Awake()
        {
            MapProbabilities();
        }

        private void MapProbabilities()
        {
            MapConversionProbs();
            MapParamRatios();
            MapTransmissionRates();

            _modelParameters = new SeirModelParameters()
            {
                exposedToInfectiousRate = GetProbIfNotNull(exposedToInfectiousRate),
                symptomaticRecoveryRate = GetProbIfNotNull(symptomaticRecoveryRate),
                asymptomaticRecoveryRate = GetProbIfNotNull(asymptomaticRecoveryRate),
                hospitalizationRecoveryRate = GetProbIfNotNull(hospitalizationRecoveryRate),
                criticalRecoveryRate = GetProbIfNotNull(criticalRecoveryRate),
                recoveredToSusceptibleRate = GetProbIfNotNull(recoveredToSusceptibleRate),
                vaccinatedToSusceptibleRate = GetProbIfNotNull(vaccinatedToSusceptibleRate),

                hospToNonHospRatio = GetProbIfNotNull(hospToNonHospRatio),
                asympToSympRatio = GetProbIfNotNull(asympToSympRatio),
                criticalToHospRatio = GetProbIfNotNull(criticalToHospRatio),
                deceasedToCriticalRecovRatio = GetProbIfNotNull(deceasedToCriticalRecovRatio),

                sympTransmissionRate = GetProbIfNotNull(sympTransmissionRate),
                asympTransmissionRate = GetProbIfNotNull(asympTransmissionRate),
                hospitalTransmissionRate = GetProbIfNotNull(hospitalTransmissionRate),
                criticalTransmissionRate = GetProbIfNotNull(criticalTransmissionRate),
                visitorTransmissionRate = GetProbIfNotNull(visitorTransmissionRate),
            };
        }

        private void MapConversionProbs()
        {
            SetConversionRate(exposedToInfectiousRate, _conversionProbs.exposedToInfectiousRate);
            SetConversionRate(symptomaticRecoveryRate, _conversionProbs.symptomaticRecoveryRate);
            SetConversionRate(asymptomaticRecoveryRate, _conversionProbs.asymptomaticRecoveryRate);
            SetConversionRate(hospitalizationRecoveryRate, _conversionProbs.hospitalizationRecoveryRate);
            SetConversionRate(criticalRecoveryRate, _conversionProbs.criticalRecoveryRate);
            SetConversionRate(recoveredToSusceptibleRate, _conversionProbs.recoveredToSusceptibleRate);
            SetConversionRate(vaccinatedToSusceptibleRate, _conversionProbs.vaccinatedToSusceptibleRate);
        }

        private void MapParamRatios()
        {
            SetParamRatio(hospToNonHospRatio, _paramRatios._hospToNonHospProportion);
            SetParamRatio(asympToSympRatio, _paramRatios._asympToSympProportion);
            SetParamRatio(criticalToHospRatio, _paramRatios._criticalToHospProportion);
            SetParamRatio(deceasedToCriticalRecovRatio, _paramRatios._deceasedToCriticalRecovProportion);
        }

        private void MapTransmissionRates()
        {
            SetTransmissionRate(sympTransmissionRate, _transmissionRates.sympTransmissionRate);
            SetTransmissionRate(asympTransmissionRate, _transmissionRates.asympTransmissionRate);
            SetTransmissionRate(hospitalTransmissionRate, _transmissionRates.hospitalTransmissionRate);
            SetTransmissionRate(criticalTransmissionRate, _transmissionRates.criticalTransmissionRate);
            SetTransmissionRate(visitorTransmissionRate, _transmissionRates.visitorTransmissionRate);
        }


        private void SetConversionRate(Connector connector, ConversionProbability conversionProbability)
        {
            if (connector is null)
            {
                return;
            }

            if (conversionProbability is null)
            {
                connector.baselineProbability = 1f;
                return;
            }

            connector.baselineProbability = conversionProbability.conversionRate;
            connector.probability = conversionProbability.conversionRate;
        }

        private void SetParamRatio(Connector connector, ParameterProportion proportion)
        {
            if (connector is null)
            {
                return;
            }

            if (proportion is null)
            {
                connector.baselineProbability = 1f;
                return;
            }

            connector.baselineProbability = proportion.proportion;
            connector.probability = proportion.proportion;
        }

        private void SetTransmissionRate(Connector connector, TransmissionProbability conversionProbability)
        {
            if (connector is null)
            {
                return;
            }

            if (conversionProbability is null)
            {
                connector.baselineProbability = 1f;
                return;
            }

            connector.baselineProbability = conversionProbability.rate;
            connector.probability = conversionProbability.rate;
        }

        private float GetProbIfNotNull(Connector connector)
        {
            return connector == null ? -1f : connector.baselineProbability;
        }
    }

    [Serializable]
    public class SeirModelParameters
    {
        [Title("Conversion Rates")] [LabelWidth(180)]
        public float exposedToInfectiousRate;

        [LabelWidth(180)] public float symptomaticRecoveryRate;
        [LabelWidth(180)] public float asymptomaticRecoveryRate;
        [LabelWidth(180)] public float hospitalizationRecoveryRate;
        [LabelWidth(180)] public float criticalRecoveryRate;
        [LabelWidth(180)] public float recoveredToSusceptibleRate;
        [LabelWidth(180)] public float vaccinatedToSusceptibleRate;

        [Title("Ratios")] [LabelWidth(180)] public float hospToNonHospRatio;
        [LabelWidth(180)] public float asympToSympRatio;
        [LabelWidth(180)] public float criticalToHospRatio;
        [LabelWidth(180)] public float deceasedToCriticalRecovRatio;

        [Title("Transmission Rates")] [LabelWidth(180)]
        public float sympTransmissionRate;

        [LabelWidth(180)] public float asympTransmissionRate;
        [LabelWidth(180)] public float hospitalTransmissionRate;
        [LabelWidth(180)] public float criticalTransmissionRate;
        [LabelWidth(180)] public float visitorTransmissionRate;

        [Title("Testing Rates")] [LabelWidth(180)]
        public float susceptibleTestingRate;

        [LabelWidth(180)] public float exposedTestingRate;
        [LabelWidth(180)] public float symptomaticTestingRate;
        [LabelWidth(180)] public float asymptomaticTestingRate;
        [LabelWidth(180)] public float hospitalizationTestingRate;
        [LabelWidth(180)] public float criticalTestingRate;
        [LabelWidth(180)] public float recoveredTestingRate;
        [LabelWidth(180)] public float vaccinatedTestingRate;

        [Title("False Test Result Rates")] [LabelWidth(180)]
        public float falsePosRate;

        [LabelWidth(180)] public float falseNegRate;
    }
}