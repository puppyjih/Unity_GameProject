using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Text myScore;
    [SerializeField] private Button LoginButton;
    [SerializeField] private GameObject AuthPanel;
    [SerializeField] private Text UserName;

    // Start is called before the first frame update
    void Start()
    {
        Func<int, int, int> Max = (x, y) => (x > y ? x : y);
        int score = 0;
        if (PlayerPrefs.HasKey(Constants.MYHIGHSCORE))
        {
            score = PlayerPrefs.GetInt(Constants.MYHIGHSCORE);
        }
        myScore.text = score.ToString();
        LoginButton.onClick.AddListener(() => AuthPanel.gameObject.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
//        PlayerPrefs.DeleteKey(Constants.USER);
        string user = PlayerPrefs.GetString(Constants.USER, "");
        if(user == "") {
            LoginButton.gameObject.SetActive(true);
            UserName.gameObject.SetActive(false);
        } else {
            LoginButton.gameObject.SetActive(false);
            UserName.gameObject.SetActive(true);
            UserName.text = user;
        }
    }
}