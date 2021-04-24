using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
using Audio;

namespace UI {
    public class ListItem : MonoBehaviour, IPointerEnterHandler {
        public AudioClip hover;
        public AudioClip click;
        public string content;

        private TextMeshProUGUI textMesh;

        [OnInspectorGUI]
        void Awake() {
            this.textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        [OnInspectorGUI]
        public void UpdateComponent() {
            this.textMesh.text = content;
        }

        public void Click() {
            AudioManager.Instance.PlaySfx(click);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            AudioManager.Instance.PlaySfx(hover);
        }
    }
}
