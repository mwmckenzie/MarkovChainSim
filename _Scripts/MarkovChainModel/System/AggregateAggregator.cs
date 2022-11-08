using System;
using System.Collections.Generic;
using System.Linq;
using KenzTools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    public class AggregateAggregator: MonoBehaviour {

        public List<DataAggregator> dataAggregators;
        public List<ProbabilityHistoryAggregator> probabilityHistoryAggregators;

        [HorizontalGroup("ModelExportSplit", .6f), Button( "Export Data [JSON]",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void ExportModelDataButton() {

            var aggregate = new AggregatedAggregate();

            foreach (var dataAggregator in dataAggregators) {
                aggregate.AddRecordDataCollection(dataAggregator.dataCollection);
            }
            foreach (var probabilityHistoryAggregator in probabilityHistoryAggregators) {
                aggregate.AddRecordDataCollection(probabilityHistoryAggregator.dataCollection);
            }
            FileExportUtils.ExportJson(aggregate, "SeirModelData", "full");
        }
    }

    [Serializable]
    public class AggregatedAggregate {
        
        public List<RecordedDataHistory> aggregatedHistories;

        public AggregatedAggregate() {
            aggregatedHistories = new List<RecordedDataHistory>();
        }

        public void AddRecordDataCollection(RecordedDataCollection recordedDataCollection) {
            foreach (var history in recordedDataCollection.histories.
                         Where(history => !aggregatedHistories.Contains(history))) {
                aggregatedHistories.Add(history);
            }
        }
    }
}