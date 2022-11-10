// Markov Chain Sim -- Source.cs
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
using Sirenix.OdinInspector;

namespace MarkovChainModel
{
    public class Source : MarkovModule, ISender, ISensible
    {
        [Required]
        [Title("Event Broadcast Channels")] 
        public VoidEventChannelSO sendPoolBroadcast;

        [Title("Outflow")] 
        public List<Connector> connectorsOut;

        [Title("Values")] 
        public float pool;


        private void Start()
        {
            sendPoolBroadcast.voidEvent += OnSendPoolBroadcast;
            //cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;

            foreach (var connector in connectorsOut)
            {
                connector.RegisterSender(this);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (conductor?.sendPoolBroadcast is not null)
                sendPoolBroadcast ??= conductor.sendPoolBroadcast;
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
        }

        private void OnCleanUpBroadcast()
        {
        }
        
        private void OnSendPoolBroadcast()
        {
            Send();
        }

        public void Send()
        {
            foreach (var connector in connectorsOut)
            {
                connector.Transition(pool);
            }
            history.Add(pool);
        }

        public float GetReading()
        {
            return pool;
        }
    }
}