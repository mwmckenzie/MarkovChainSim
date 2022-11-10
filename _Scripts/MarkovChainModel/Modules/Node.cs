// Markov Chain Sim -- Node.cs
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
using UnityEngine;

namespace MarkovChainModel
{
    public class Node : MarkovModule, ISensible, ISender, IReceiver, IPumpable
    {
        [Required]
        [Title("Event Broadcast Channels")] 
        public VoidEventChannelSO sendPoolBroadcast;

        [Title("Inflow")] 
        public List<Connector> connectorsIn;

        [Title("Outflow")] [HideLabel] 
        public Connector connectorOut;

        [Title("Values")] 
        public float pool;
        public float incoming;

        private void Start()
        {
            sendPoolBroadcast.voidEvent += OnSendPoolBroadcast;
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;

            if (connectorOut != null)
                connectorOut.RegisterSender(this);
            
            RegisterAsReceiver();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (conductor?.sendPoolBroadcast is not null)
                sendPoolBroadcast ??= conductor.sendPoolBroadcast;
            
            connectorOut = GetComponentInChildren<Connector>();
            RegisterAsReceiver();
        }

        private void OnCleanUpBroadcast()
        {
            pool = incoming;
            incoming = 0;
            history.Add(pool);
        }

        private void OnSendPoolBroadcast()
        {
            Send();
        }

        public void Receive(float val)
        {
            incoming += val;
        }

        public void RegisterAsReceiver()
        {
            foreach (var connector in connectorsIn)
            {
                connector.RegisterReciever(this);
            }
        }

        public void Send()
        {
            if (connectorOut is null)
            {
                incoming += pool;
                return;
            }

            connectorOut.Transition(pool);
            pool = 0;
        }

        public float GetReading()
        {
            return pool;
        }

        public float Pump(float val)
        {
            var amount = Mathf.Min(val, pool);
            pool -= amount;
            return amount;
        }
    }
}