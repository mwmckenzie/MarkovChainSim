using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "FloatListEventChannelSO_", menuName = "Scriptable Object/Event Channels/Float List Event", order = 0)]
    public class FloatListEventChannel : ScriptableObject {
        
        public UnityAction<List<float>> floatListEvent;

        public void RaiseEvent(List<float> policySlots) {
            floatListEvent?.Invoke(policySlots);
        }
    }
}