using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

namespace MainMenu {
    public class MainMenu : MonoBehaviour {
        private void Awake() {
            AudioManager.Instance.PlayMusic(MusicType.MainTheme);
        }
    }
}

