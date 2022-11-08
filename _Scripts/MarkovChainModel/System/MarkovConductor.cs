using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MarkovChainModel {
    public class MarkovConductor : SerializedMonoBehaviour {
        
        
        public VoidEventChannelSO takeReadingBroadcast;
        public VoidEventChannelSO sendReadingBroadcast;
        public VoidEventChannelSO sendPoolBroadcast;
        //public VoidEventChannelSO sendSplitsBroadcast;
        
        public VoidEventChannelSO cleanUpBroadcast;
        public VoidEventChannelSO recordDataBroadcast;
        
        [HorizontalGroup("EventsSplit", .5f), Button("Broadcast Channels",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(60)]
        private void BroadcastChannels() {
            if (cleanUpBroadcast is null) {
                return;
            }
            var modules = FindObjectsOfType<MarkovModule>();
            foreach (var module in modules) {
                module.cleanUpBroadcast = cleanUpBroadcast;
            }
        }

        [SerializeField] private int _stepsPerSimFFwd = 30;
        [ShowInInspector] public int step;

        private int _requestedStep;
        private bool isBusy;
        private List<VoidEventChannelSO> _broadcasts;

        [HorizontalGroup("TopSplit", .5f), Button("Run Step",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(60)]
        private void SimFwdButton() {
            OrchestrateNextSteps(1);
        }
        
        [HorizontalGroup("TopSplit", .5f), Button("Run Step",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(60)]
        private void SimFFwdButton() {
            OrchestrateNextSteps(_stepsPerSimFFwd);
        }

        private void Awake() {
            
            _broadcasts = new List<VoidEventChannelSO>() {
                takeReadingBroadcast,
                sendReadingBroadcast,
                sendPoolBroadcast,
                cleanUpBroadcast,
                recordDataBroadcast
            };
            
        }

        public void OrchestrateNextSteps(int steps) {
            // takeReadingBroadcast.RaiseEvent();
            // sendReadingBroadcast.RaiseEvent();
            // sendPoolBroadcast.RaiseEvent();
            // //sendSplitsBroadcast.RaiseEvent();
            // cleanUpBroadcast.RaiseEvent();
            // recordDataBroadcast.RaiseEvent();
            
            if (isBusy) {
                return;
            }

            _requestedStep += steps;
            
            StartCoroutine(BroadcastSequence());
            
        }
        
        private IEnumerator BroadcastSequence() {

            isBusy = true;

            while (step < _requestedStep) {
                foreach (var broadcast in _broadcasts) {
                    broadcast.RaiseEvent();

                    yield return null;
                }

                step++;
                yield return null;
            }
            
            isBusy = false;
        }
    }
}