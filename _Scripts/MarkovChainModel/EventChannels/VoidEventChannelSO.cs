using UnityEngine;
using UnityEngine.Events;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "VoidEventChannel_", menuName = "Scriptable Object/Event Channels/Void Event", order = 0)]
    public class VoidEventChannelSO : ScriptableObject {
        
        public UnityAction voidEvent;

        public void RaiseEvent() {
            voidEvent?.Invoke();
        }
    }
}