using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace utils {
    public class SceneManagement : MonoBehaviour {
        public void ChangeScene(int sceneIndex) {
            LoadingScreenManager.LoadScene(sceneIndex);
        }
    }
}

