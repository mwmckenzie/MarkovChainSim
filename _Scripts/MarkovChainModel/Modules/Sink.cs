// Markov Chain Sim -- Sink.cs
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

namespace MarkovChainModel
{
    public class Sink : MarkovModule, IReceiver, ISensible
    {
        [Title("Inflow")] 
        public List<Connector> connectorsIn;

        [Title("Values")] 
        public float pool;

        private float lastValue;

        private void Start()
        {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            RegisterAsReceiver();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            RegisterAsReceiver();
        }

        private void OnCleanUpBroadcast()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            lastValue = pool;
            history.Add(pool);
            pool = 0;
        }

        public void Receive(float val)
        {
            pool += val;
        }

        public void RegisterAsReceiver()
        {
            foreach (var connector in connectorsIn)
            {
                connector.RegisterReciever(this);
            }
        }

        public float GetReading()
        {
            return lastValue;
        }
    }
}