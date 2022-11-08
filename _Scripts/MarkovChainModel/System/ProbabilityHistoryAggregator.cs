using System.Collections.Generic;
using KenzTools;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class ProbabilityHistoryAggregator : SerializedMonoBehaviour {
        
        [HorizontalGroup("ModelExportSplit", .6f), Button( "Export Data [JSON]",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void ExportModelDataButton() {
            FileExportUtils.ExportJson(dataCollection, "SimProbHist", dataCollectionName);
        }

        [Title("Collection Name"), HideLabel]
        public string dataCollectionName;
        
        [Title("Record Data Broadcast Channel")]
        public VoidEventChannelSO recordDataBroadcast;
        
        [Title("Modules To Record")]
        public List<Connector> connectorsToRecord;
        
        [Title("Current Values")]
        public Dictionary<string, float> moduleCurrProbabilities;

        [Title("Data Collection")][HideLabel]
        public RecordedDataCollection dataCollection;
        

        private void Start() {
            recordDataBroadcast.voidEvent += OnRecordDataBroadcast;
        }

        private void OnRecordDataBroadcast() {
            dataCollection = new RecordedDataCollection(dataCollectionName);

            foreach (var node in connectorsToRecord) {
                var module = new RecordedDataHistory(node.name) {
                    values = node.historyProbability
                };
                dataCollection.histories.Add(module);
            }
            
            moduleCurrProbabilities = new Dictionary<string, float>();

            foreach (var history in dataCollection.histories) {
                
                var val = history.LatestValue();
                var histName = history.field;
                
                moduleCurrProbabilities.Add(histName, val);
                
            }
        }
    }
}