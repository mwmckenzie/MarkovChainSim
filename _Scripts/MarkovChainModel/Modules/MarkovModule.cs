using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MarkovChainModel {
    public class MarkovModule : SerializedMonoBehaviour {
        
        [Title("Name"), HideLabel]
        public new string name;
        
        [Title("Event Broadcast Channels")][Required]
        public VoidEventChannelSO cleanUpBroadcast;
        
        [Title("Notes")]
        [HideLabel, MultiLineProperty(3)]
        public string notes = "";

        [Title("History")] 
        public List<float> history;
        
        [HorizontalGroup("ModelExportSplit", 1f), Button( "Create Output Connector",ButtonSizes.Large)]
        [GUIColor(.4f, .8f, 1f), PropertyOrder(5)]
        private void CreateOutputConnecterButton() {
            var go = new GameObject("CreatedGameObject");
            go.transform.parent = this.transform;
            go.AddComponent<Connector>();
        }

        private void Awake() {
            SetNameIfNotNothing();
            history = new List<float>();
        }
        
        protected virtual void OnValidate() {
            SetNameIfNotNothing();
        }

        private void SetNameIfNotNothing() {
            if (!gameObject.name.IsNullOrWhitespace()) {
                name = gameObject.name;
            }
        }
    }
}