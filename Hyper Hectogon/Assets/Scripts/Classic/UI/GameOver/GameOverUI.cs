using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField]
    private Button _Retry, _GoMenu;
    // Start is called before the first frame update
    void Start() {
        _Retry.onClick.AddListener(() => Scene.goScene("Classic"));
        _GoMenu.onClick.AddListener(() => Scene.goScene("Menu"));
    }

    void SetHighScore() {
        Func<int, int, int> Max = (x, y) => (x > y ? x : y);
        int score = 0;
        if (PlayerPrefs.HasKey(Constants.MYHIGHSCORE))
        {
            score = Max(score, PlayerPrefs.GetInt(Constants.MYHIGHSCORE));
        }
        PlayerPrefs.SetInt(Constants.MYHIGHSCORE, score);
    }
}
