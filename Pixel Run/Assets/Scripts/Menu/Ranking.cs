using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Ranking : MonoBehaviour
{
    [SerializeField] private Transform RankParent;
    [SerializeField] private GameObject RankObject;
    // Start is called before the first frame update
    void Start()
    {
        ServerManager.RankingWithFriend(this, PlayerPrefs.GetString(Constants.USER, ""));
    }

    // Update is called once per frame
    void Update()
    {
        fetchingRanking();
    }

    void fetchingRanking() {
        if(!ServerManager.isFetching.Ranking) return;
        JObject ranks = JObject.Parse(ServerManager.Data);
        ServerManager.isFetching.Ranking = false;
        GameObject rankObject;
        foreach(var obj in ranks["user"]) {
            Debug.Log(obj["username"] + " " + obj["highScore"]);
            rankObject = Instantiate(RankObject);
            rankObject.transform.GetChild(0).GetComponent<Text>().text = obj["username"].ToString();
            rankObject.transform.GetChild(1).GetComponent<Text>().text = obj["highScore"].ToString();
            rankObject.transform.parent = RankParent;
            rankObject.SetActive(true);
        }
    }
}
