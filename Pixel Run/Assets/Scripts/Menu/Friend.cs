using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Friend : MonoBehaviour
{
    [SerializeField] private InputField FindQuery;
    [SerializeField] private Button FindButton;
    [SerializeField] private Text FriendName;
    [SerializeField] private Text FriendHighScore;
    [SerializeField] private Button AddFriendButton;
    [SerializeField] private Button Close;

    // Start is called before the first frame update
    void Start()
    {
        FindButton.onClick.AddListener(FindFriend);
        AddFriendButton.onClick.AddListener(AddFriend);
        Close.onClick.AddListener(() => PanelManager.instance.PanelClose("Friend"));
    }

    // Update is called once per frame
    void Update()
    {
        if(!ServerManager.isFetching.AddFriend) return;
        ServerManager.isFetching.AddFriend = false;
        JObject friend = JObject.Parse(ServerManager.Data);
        FriendName.text = friend["user"]["username"].ToString();
        FriendHighScore.text = friend["user"]["highScore"].ToString();
    }

    private void FindFriend() {
        ServerManager.FindFriend(this, FindQuery.text);
    }

    private void AddFriend() {
        ServerManager.AddFriend(this, PlayerPrefs.GetString(Constants.USER, ""), FriendName.text);
        ServerManager.isFetching.AddFriend = false;
    }
}
