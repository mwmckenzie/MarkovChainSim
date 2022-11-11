// Markov Chain Sim -- Pump.cs
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

using Sirenix.OdinInspector;

namespace MarkovChainModel
{
    public class Pump : MarkovModule, IReceiver, ISender
    {
        [Required]
        [Title("Pump Source Node")] [HideLabel] 
        public IPumpable pumpSource;

        [Required]
        [Title("Signal In")] [HideLabel] 
        public Connector connectorIn;

        [Required]
        [Title("Pump Outflow")] [HideLabel] 
        public Connector connectorOut;

        [Title("Values")] 
        public float pool;
        public float signal;


        private void Start()
        {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;

            connectorOut.RegisterSender(this);
            RegisterAsReceiver();
        }

        protected override void OnValidate()
        {
            connectorOut = GetComponentInChildren<Connector>();
            RegisterAsReceiver();
            base.OnValidate();
        }

        private void OnCleanUpBroadcast()
        {
        }


        public void Receive(float val)
        {
            signal = val;
            PumpFromSource();
        }

        public void RegisterAsReceiver()
        {
            connectorIn.RegisterReceiver(this);
        }

        public void PumpFromSource()
        {
            pool = pumpSource.Pump(signal);
            Send();
        }

        public void Send()
        {
            connectorOut.Transition(pool);
            pool = 0;
        }
    }
}