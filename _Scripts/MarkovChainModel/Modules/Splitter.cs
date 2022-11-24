// Markov Chain Sim -- Splitter.cs
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
using KenzTools;
using Sirenix.OdinInspector;

namespace MarkovChainModel
{
    public class Splitter : MarkovModule, IReceiver, ISender
    {
        [Title("Flow In")] 
        public List<Connector> connectorsIn;

        [Title("Split Outflow")] 
        public List<Connector> connectorsOut;

        [Title("Values")] 
        public float pool;
        public float incoming;

        private void Start()
        {
            foreach (var connector in connectorsOut)
            {
                connector.RegisterSender(this);
            }
            RegisterAsReceiver();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            RegisterAsReceiver();
            UpdateOutputsFromChildren();
            OrderOutputsByProbability();
        }

        private void UpdateOutputsFromChildren()
        {
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
        }
        
        public void Receive(float val)
        {
            pool += val;
            Send();
        }

        public void RegisterAsReceiver()
        {
            foreach (var connector in connectorsIn)
            {
                connector.RegisterReceiver(this);
            }
        }

        private void OrderOutputsByProbability()
        {
            connectorsOut = connectorsOut.OrderBy(x => x.probability).ToList();
            
            switch (connectorsOut.Count)
            {
                case 0:
                    return;
                case 1:
                    return;
                case 2:
                    if (connectorsOut[1].probability >= 1)
                    {
                        return;
                    }
                    break;
            }
            
            for (var i = 1; i < connectorsOut.Count - 1; i++)
            {
                var lesserProb = connectorsOut[i - 1].probability;
                connectorsOut[i].probability = connectorsOut[i].baselineProbability * 1f / (1f - lesserProb);
            }
        }

        public void Send()
        {
            OrderOutputsByProbability();
            
            foreach (var connector in connectorsOut)
            {
                if (pool > 0)
                {
                    var output = 
                        RandomUtils.CalculateTransitionCount(pool, connector.probability);
                    connector.Transition(output);
                    pool -= output;
                }
                else
                {
                    connector.Transition(0);
                }
            }
            pool = 0;
        }
    }
}