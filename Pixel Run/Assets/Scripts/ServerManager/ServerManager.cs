using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;

public class isFetchingClass {
    public bool Login;
    public bool Register;
    public bool FindFriend;
    public bool AddFriend;
    public bool HighScore;
    public bool Ranking;
    public isFetchingClass() {
        Login = false;
        Register = false;
        FindFriend = false;
        AddFriend = false;
        HighScore = false;
        Ranking = false;
    }
}
public class ServerManager : MonoBehaviour {
    delegate void CallBack(Dictionary<string, string> dic);

    public static string Data = "";
    public static bool OK = false;
    public static isFetchingClass isFetching = new isFetchingClass();

    // Find Friend
    public static void FindFriendSuccess(Dictionary<string, string> dic) {
        isFetching.FindFriend = true;
    }

    public static void FindFriendFail() {
        isFetching.FindFriend = false;
    }

    public static void FindFriend(MonoBehaviour __mono__, string name) {
        OK = false;
        __mono__.StartCoroutine(GetRequest("users/find/" + name, new Dictionary<string, string>(), new CallBack(FindFriendSuccess)));
    }

    // High Score
    public static void UpdateHighScoreSuccess(Dictionary<string, string> dic) {
        isFetching.HighScore = true;
    }
    public static void UpdateHighScore(MonoBehaviour __mono__, string username, int highScore) {
        if(username.Equals("")) return;
        __mono__.StartCoroutine(PostRequest("users/update/score", new Dictionary<string, string>{{"username", username}, {"highScore", highScore.ToString()}}, new CallBack(UpdateHighScoreSuccess)));
    }

    // Ranking
    public static void RankingWithFriendSuccess(Dictionary<string, string> dic) {
        Debug.Log("init");
        isFetching.Ranking = true;
    }

    public static void RankingWithFriend(MonoBehaviour __mono__, string username) {
        if(username.Equals("")) return;
        __mono__.StartCoroutine(PostRequest("rankings/my", new Dictionary<string, string>{{"username", username}}, new CallBack(RankingWithFriendSuccess)));
    }

    // Login
    public static void LoginSuccess(Dictionary<string, string> dic) {
        PlayerPrefs.SetString(Constants.USER, dic["username"]);
        PanelManager.instance.PanelClose("Auth");
        isFetching.Login = true;
    }
    // public static void Login(MonoBehaviour __mono__, string username, string password) {
    //     __mono__.StartCoroutine(PostRequest("auth/login", new Dictionary<string, string>{{"username", username}, {"password", password}}, true));
    // }
    public static void Login(MonoBehaviour __mono__, string username, string password) {
        __mono__.StartCoroutine(PostRequest("auth/login", new Dictionary<string, string>{{"username", username}, {"password", password}}, new CallBack(LoginSuccess)));
    }

    // Register Part
    public static void RegisterSuccess(Dictionary<string, string> dic) {
        isFetching.Register = true;
    }
    public static void Register(MonoBehaviour __mono__, string username, string password) {
        __mono__.StartCoroutine(PostRequest("auth/register", new Dictionary<string, string>{{"username", username}, {"password", password}}, new CallBack(RegisterSuccess)));
    }

    // Add Friend
    public static void AddFriendSuccess(Dictionary<string, string> dic) {
        isFetching.AddFriend = true;
    }
    public static void AddFriend(MonoBehaviour __mono__, string username, string friend) {
        __mono__.StartCoroutine(PostRequest("users/friend/add", new Dictionary<string, string>{{"user1", username}, {"user2", friend}}, new CallBack(AddFriendSuccess)));
    }

    private static IEnumerator GetRequest(string option, Dictionary<string, string> dic, CallBack SuccessCallBack) {
        var data = JsonConvert.SerializeObject(dic);
        using (UnityWebRequest request = UnityWebRequest.Get(Constants.SERVER_DOMAIN + option)) {
            Debug.Log("init");
            request.method = UnityWebRequest.kHttpVerbGET;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if(!request.isNetworkError && request.responseCode == 200) {
                Data = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log("Success");
                Debug.Log(request.downloadHandler.text);
                Debug.Log(System.Text.Encoding.UTF8.GetString(request.downloadHandler.data));
                SuccessCallBack(dic);
            } else {
                Debug.Log(request.isNetworkError);
                Debug.Log(request.responseCode);
                Debug.Log("Fail");
            }
        }
        yield return 0;
    }

    private static IEnumerator PostRequest(string option, Dictionary<string, string> dic, CallBack SuccessCallBack) {
        string data = JsonConvert.SerializeObject(dic);
        Debug.Log(data);
        using (UnityWebRequest request = UnityWebRequest.Post(Constants.SERVER_DOMAIN + option, data)) {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            yield return request.SendWebRequest();
            if(!request.isNetworkError && request.responseCode == 200) {
                Debug.Log("ing");
                Data = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                OK = true;
                SuccessCallBack(dic);
            } else {
                Debug.Log(request.isNetworkError);
                Debug.Log(request.responseCode);
                Debug.Log(System.Text.Encoding.UTF8.GetString(request.downloadHandler.data));
                Debug.Log("Fail");
            }
        }
        yield return 0;
    }
}