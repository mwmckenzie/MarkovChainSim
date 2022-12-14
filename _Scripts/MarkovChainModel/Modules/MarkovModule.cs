// Markov Chain Sim -- MarkovModule.cs
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
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MarkovChainModel
{
    public abstract class MarkovModule : SerializedMonoBehaviour
    {
        [Title("Name"), HideLabel] 
        public new string name;

        [Required]
        [Title("Event Broadcast Channels")] 
        public VoidEventChannelSO cleanUpBroadcast;

        [Title("Notes")] 
        [HideLabel, MultiLineProperty(3)]
        public string notes = string.Empty;

        [Title("History")] public List<float> history;

        [HorizontalGroup("ModelExportSplit", 1f)]
        [Button("Create Output Connector", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void CreateOutputConnectorButton()
        {
            var go = new GameObject("OutputConnector")
            {
                transform =
                {
                    parent = transform
                }
            };
            go.AddComponent<Connector>();
        }

        protected MarkovConductor conductor;
        
        protected void Awake()
        {
            SetNameIfNotNothing();
            history = new List<float>();
        }

        protected virtual void OnValidate()
        {
            SetNameIfNotNothing();
            conductor ??= GetComponentInParent<MarkovConductor>(true);
            if (conductor?.cleanUpBroadcast is not null)
                cleanUpBroadcast ??= conductor.cleanUpBroadcast;
        }

        private void SetNameIfNotNothing()
        {
            if (!gameObject.name.IsNullOrWhitespace())
            {
                name = gameObject.name;
            }
        }
    }
}