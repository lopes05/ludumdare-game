using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace UI {
    public class List : MonoBehaviour {
        public ListItem[] items;
        public Action callback;

        [OnInspectorGUI]
        public void UpdateChildren() {
            ListItem[] children = GetComponentsInChildren<ListItem>();

            this.items = children;
        }
    }    
}
