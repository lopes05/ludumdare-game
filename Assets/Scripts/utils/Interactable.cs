using UnityEngine;

namespace utils {
    public abstract class Interactable : MonoBehaviour {
        public abstract void SetButton(bool active);
        public abstract bool HandleClick(GameObject obj);
    }
}