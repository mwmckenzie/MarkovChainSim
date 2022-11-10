// Markov Chain Sim -- Sensor.cs
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

namespace MarkovChainModel
{
    public class Sensor : MarkovModule, ISender
    {
        [Required]
        [Title("Event Broadcast Channels")] 
        public VoidEventChannelSO takeReadingBroadcast;

        [Required] 
        public VoidEventChannelSO sendReadingBroadcast;

        [Required]
        [Title("Monitored Object")] [HideLabel] 
        public ISensible monitoredObj;

        [Title("Signal(s) Out")] 
        public List<Connector> connectorsOut;

        [Title("Values")] 
        public float reading;

        private void Start()
        {
            takeReadingBroadcast.voidEvent += OnTakeReadingBroadcast;
            sendReadingBroadcast.voidEvent += OnSendReadingBroadcast;
            foreach (var connector in connectorsOut)
            {
                connector.RegisterSender(this);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (conductor?.takeReadingBroadcast is not null)
                takeReadingBroadcast ??= conductor.takeReadingBroadcast;
            if (conductor?.sendReadingBroadcast is not null)
                sendReadingBroadcast ??= conductor.sendReadingBroadcast;
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
        }

        private void OnSendReadingBroadcast()
        {
            Send();
        }

        private void OnTakeReadingBroadcast()
        {
            reading = monitoredObj.GetReading();
            history.Add(reading);
        }

        public void Send()
        {
            foreach (var connector in connectorsOut)
            {
                connector.Transition(reading);
            }
        }
    }
}