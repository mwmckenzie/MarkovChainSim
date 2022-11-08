using UnityEngine;
using UnityEngine.Events;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "IntEventChannel_", menuName = "Scriptable Object/Event Channels/Int Event", order = 0)]
    public class IntEventChannelSO : ScriptableObject {

        public UnityAction<int> intEvent;

        public void RaiseEvent(int val) {
            intEvent?.Invoke(val);
        }
    }
}