// Markov Chain Sim -- MarkovElementBuilder.cs
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
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel
{
    public class MarkovElementBuilder : MonoBehaviour
    {
        [Title("Template Build Options")]
        private List<string> _typeParentNames = new List<string>()
        {
            "Nodes",
            "Splitters",
            "Adjusters",
            "Sensors",
            "Pumps",
            "Sinks",
            "Sources"
        };
        
        [HorizontalGroup("EventsSplit", 1f)]
        [Button("Add New Element", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(60)]
        private void BuildMissingPrimaryGameObjects()
        {
            var typeName = gameObject.name;
            if (!_typeParentNames.Contains(typeName))
            {
                return;
            }

            var go = new GameObject("MarkovModule"){
                transform =
                {
                    parent = transform
                }
            };
            
            switch (typeName)
            {
                    case "Nodes":
                        go.name = typeName[..^1];
                        go.AddComponent<Node>();
                        break;
                    case "Splitters":
                        go.name = typeName[..^1];
                        go.AddComponent<Splitter>();
                        break;
                    case "Adjusters":
                        go.name = typeName[..^1];
                        go.AddComponent<Adjuster>();
                        break;
                    case "Sensors":
                        go.name = typeName[..^1];
                        go.AddComponent<Sensor>();
                        break;
                    case "Pumps":
                        go.name = typeName[..^1];
                        go.AddComponent<Pump>();
                        break;
                    case "Sinks":
                        go.name = typeName[..^1];
                        go.AddComponent<Sink>();
                        break;
                    case "Sources":
                        go.name = typeName[..^1];
                        go.AddComponent<Source>();
                        break;
            }
        }
    }
}