using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {
    public static void goScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
