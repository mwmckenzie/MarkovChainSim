using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MarkovChainModel {
    [CreateAssetMenu(fileName = "IntListEventChannel_", menuName = "Scriptable Object/Event Channels/Int List Event", order = 0)]
    public class IntListEventChannel : ScriptableObject {
        public UnityAction<List<int>> intListEvent;

        public void RaiseEvent(List<int> list) {
            intListEvent?.Invoke(list);
        }
    }
}