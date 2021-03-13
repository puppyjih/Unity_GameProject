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
    [SerializeField] private GameObject Logined;
    [SerializeField] private Text UserName;
    [SerializeField] private Button AddFriendButton;
    private bool isLogined = false;
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
        ServerManager.UpdateHighScore(this, PlayerPrefs.GetString(Constants.USER, ""), score);
        LoginButton.onClick.AddListener(() => AuthPanel.gameObject.SetActive(true));
        AddFriendButton.onClick.AddListener(() => PanelManager.instance.PanelOpen("Friend"));
    }

    // Update is called once per frame
    void Update()
    {
        if(isLogined) return;
        string user = PlayerPrefs.GetString(Constants.USER, "");
        if(user == "") {
            LoginButton.gameObject.SetActive(true);
            Logined.SetActive(false);
        } else {
            LoginButton.gameObject.SetActive(false);
            Logined.SetActive(true);
            UserName.text = user;
            isLogined = true;
        }
    }
}