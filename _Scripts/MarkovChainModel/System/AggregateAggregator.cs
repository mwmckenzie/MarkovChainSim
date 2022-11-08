// Markov Chain Sim -- AggregateAggregator.cs
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
using System.Linq;
using KenzTools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel
{
    public class AggregateAggregator : MonoBehaviour
    {
        public List<DataAggregator> dataAggregators;
        public List<ProbabilityHistoryAggregator> probabilityHistoryAggregators;

        [HorizontalGroup("ModelExportSplit", .6f), Button("Export Data [JSON]", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void ExportModelDataButton()
        {
            var aggregate = new AggregatedAggregate();

            foreach (var dataAggregator in dataAggregators)
            {
                aggregate.AddRecordDataCollection(dataAggregator.dataCollection);
            }

            foreach (var probabilityHistoryAggregator in probabilityHistoryAggregators)
            {
                aggregate.AddRecordDataCollection(probabilityHistoryAggregator.dataCollection);
            }

            FileExportUtils.ExportJson(aggregate, "SeirModelData", "full");
        }
    }

    [Serializable]
    public class AggregatedAggregate
    {
        public List<RecordedDataHistory> aggregatedHistories;

        public AggregatedAggregate()
        {
            aggregatedHistories = new List<RecordedDataHistory>();
        }

        public void AddRecordDataCollection(RecordedDataCollection recordedDataCollection)
        {
            foreach (var history in recordedDataCollection.histories.Where(history =>
                         !aggregatedHistories.Contains(history)))
            {
                aggregatedHistories.Add(history);
            }
        }
    }
}