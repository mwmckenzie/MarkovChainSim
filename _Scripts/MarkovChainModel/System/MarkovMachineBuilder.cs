// Markov Chain Sim -- MarkovMachineBuilder.cs
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
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

namespace MarkovChainModel
{
    public class MarkovMachineBuilder : MonoBehaviour
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
        [Button("Build Primary Game Objects", ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(60)]
        private void BuildMissingPrimaryGameObjects()
        {
            var children = GetComponentsInChildren<Transform>().ToList();
            foreach (var typeParentName  in _typeParentNames)
            {
                
                if (children.Any(x => x.name == typeParentName))
                {
                    var child = children.First(x => x.name == typeParentName);
                    var elementBuilder = child.GetComponent<MarkovElementBuilder>();
                    
                    if (elementBuilder is null)
                    {
                        child.AddComponent<MarkovElementBuilder>();
                    }
                    continue;
                }
                
                var go = new GameObject(typeParentName)
                {
                    transform =
                    {
                        parent = transform
                    }
                };
                go.AddComponent<MarkovElementBuilder>();
            }
        }

        
    }
}