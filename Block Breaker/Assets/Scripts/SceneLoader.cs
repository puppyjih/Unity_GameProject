using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void NextSceneLoader()
    {
        int curSceneNum = SceneManager.GetActiveScene().buildIndex;
        int sceneNum = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene((curSceneNum + 1)% sceneNum);
    }
}
