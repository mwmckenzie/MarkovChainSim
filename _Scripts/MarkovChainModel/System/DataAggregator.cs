// Markov Chain Sim -- DataAggregator.cs
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
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel
{
    public class DataAggregator : SerializedMonoBehaviour
    {
        [HorizontalGroup("ModelExportSplit", .6f), Button("Export Data [JSON]", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void ExportModelDataButton()
        {
            var json = JsonUtility.ToJson(dataCollection.histories.ToArray(), true);
            var filename =
                $"ModelDataExport_{dataCollection.collectionName}_{DateTime.Now.ToString("yyyy-MM-dd-HHmm")}.json";
            File.WriteAllText($"D:/Unity/Projects/CardGameScratchSpace/Assets/data/{filename}", json);
        }

        [Title("Collection Name"), HideLabel] public string dataCollectionName;

        [Title("Record Data Broadcast Channel")]
        public VoidEventChannelSO recordDataBroadcast;

        [Title("Modules To Record")] public List<MarkovModule> nodesToRecord;

        [Title("Current Values")] public Dictionary<string, float> moduleCurrData;

        [Title("Data Collection")] [HideLabel] public RecordedDataCollection dataCollection;


        private void Start()
        {
            recordDataBroadcast.voidEvent += OnRecordDataBroadcast;
        }

        private void OnRecordDataBroadcast()
        {
            dataCollection = new RecordedDataCollection(dataCollectionName);

            foreach (var node in nodesToRecord)
            {
                var module = new RecordedDataHistory(node.name);
                module.values = node.history;
                dataCollection.histories.Add(module);
            }

            moduleCurrData = new Dictionary<string, float>();

            foreach (var history in dataCollection.histories)
            {
                var val = history.LatestValue();
                var histName = history.field;

                moduleCurrData.Add(histName, val);
            }
        }
    }

    [Serializable]
    public struct RecordedData
    {
        public string field;
        public float value;
    }

    [Serializable]
    public class RecordedDataCollection
    {
        public string collectionName;
        public List<RecordedDataHistory> histories;

        public RecordedDataCollection(string collName)
        {
            collectionName = collName;
            histories = new List<RecordedDataHistory>();
        }
    }

    [Serializable]
    public class RecordedDataHistory
    {
        public string field;
        public List<float> values;

        public RecordedDataHistory(string fieldName)
        {
            field = fieldName;
            values = new List<float>();
        }

        public void AddValue(float val)
        {
            values.Add(val);
        }

        public float LatestValue()
        {
            if (values == null)
            {
                return 0f;
            }

            return values.Count < 1 ? 0f : values[^1];
        }
    }
}