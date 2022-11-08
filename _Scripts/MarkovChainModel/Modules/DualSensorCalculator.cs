using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace MarkovChainModel {
    public class DualSensorCalculator : MarkovModule, ISender {

        [Title("Event Broadcast Channels")][Required]
        public VoidEventChannelSO takeReadingBroadcast;
        [Required]
        public VoidEventChannelSO sendReadingBroadcast;
        
        public CalculatorType type;
        
        [Title("Value A Node")][HideLabel][Required]
        public ISensible monitoredObjValueA;
        
        [Title("Value B Node")][HideLabel][Required]
        public ISensible monitoredObjValueB;
        
        [Title("Signal(s) Out")]
        public List<Connector> connectorsOut;

        private float _valueA;
        private float _valueB;
        private float _calculation;
        
        private void Start() {
            cleanUpBroadcast.voidEvent += OnCleanUpBroadcast;
            takeReadingBroadcast.voidEvent += OnTakeReadingBroadcast;
            sendReadingBroadcast.voidEvent += OnSendReadingBroadcast;
            foreach (var connector in connectorsOut) {
                connector.RegisterSender(this);
            }
        }
        
        protected override void OnValidate() {
            connectorsOut = GetComponentsInChildren<Connector>().ToList();
            foreach (var connector in connectorsOut) {
                connector.RegisterSender(this);
            }
            base.OnValidate();
        }
        
        private void OnSendReadingBroadcast() {
            Send();
        }

        private void OnTakeReadingBroadcast() {

            _valueA = monitoredObjValueA.GetReading();
            _valueB = monitoredObjValueB.GetReading();
            
            switch (type) {
                case CalculatorType.Add:
                    _calculation = _valueA + _valueB;
                    break;
                case CalculatorType.Subtract:
                    _calculation = _valueA - _valueB;
                    break;
                case CalculatorType.Multiply:
                    _calculation = _valueA * _valueB;
                    break;
                case CalculatorType.Divide:
                    if (_valueB != 0f) {
                        _calculation = _valueA / _valueB;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        private void OnCleanUpBroadcast() {
            history.Add(_calculation);
            _calculation = 0f;
            _valueA = 0f;
            _valueB = 0f;
        }

        public void Send() {
            foreach (var connector in connectorsOut) {
                connector.Transition(_calculation);
            }
        }
    }

    public enum CalculatorType {
        Add,
        Subtract,
        Multiply,
        Divide,
        
    }
}