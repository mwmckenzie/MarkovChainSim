// Markov Chain Sim -- ProbabilityHistoryAggregator.cs
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

using System.Collections.Generic;
using KenzTools;
using Sirenix.OdinInspector;

namespace MarkovChainModel
{
    public class ProbabilityHistoryAggregator : SerializedMonoBehaviour
    {
        [HorizontalGroup("ModelExportSplit", .6f), Button("Export Data [JSON]", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void ExportModelDataButton()
        {
            FileExportUtils.ExportJson(dataCollection, "SimProbHist", dataCollectionName);
        }

        [Title("Collection Name"), HideLabel] public string dataCollectionName;

        [Title("Record Data Broadcast Channel")]
        public VoidEventChannelSO recordDataBroadcast;

        [Title("Modules To Record")] public List<Connector> connectorsToRecord;

        [Title("Current Values")] public Dictionary<string, float> moduleCurrProbabilities;

        [Title("Data Collection")] [HideLabel] public RecordedDataCollection dataCollection;

        private MarkovConductor _conductor;
        
        private void OnValidate()
        {
            _conductor ??= GetComponentInParent<MarkovConductor>(true);
            if (_conductor?.recordDataBroadcast is not null)
                recordDataBroadcast ??= _conductor.recordDataBroadcast;
        }

        private void Start()
        {
            recordDataBroadcast.voidEvent += OnRecordDataBroadcast;
        }

        private void OnRecordDataBroadcast()
        {
            dataCollection = new RecordedDataCollection(dataCollectionName);

            foreach (var node in connectorsToRecord)
            {
                var module = new RecordedDataHistory(node.name)
                {
                    values = node.historyProbability
                };
                dataCollection.histories.Add(module);
            }

            moduleCurrProbabilities = new Dictionary<string, float>();

            foreach (var history in dataCollection.histories)
            {
                var val = history.LatestValue();
                var histName = history.field;

                moduleCurrProbabilities.Add(histName, val);
            }
        }
    }
}