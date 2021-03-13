using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour {
    public static void Ranking() {
        //
    }

    // Login Part
    public static void Login(MonoBehaviour __mono__, string username, string password) {
        __mono__.StartCoroutine(PostRequest("auth/login", new Dictionary<string, string>{{"username", username}, {"password", password}}, true));
    }

    // Register Part
    public static void Register(MonoBehaviour __mono__, string username, string password) {
        __mono__.StartCoroutine(PostRequest("auth/register", new Dictionary<string, string>{{"username", username}, {"password", password}}));
    }

    public static void RegisterSuccess() {
        //
    }

    public static void RegisterFailed() {
        //
    }

    private static IEnumerator GetRequest(string option, Dictionary<string, string> dic) {
        var data = JsonConvert.SerializeObject(dic);
        using (UnityWebRequest request = UnityWebRequest.Get(Constants.SERVER_DOMAIN + option)) {
            Debug.Log("init");
            request.method = UnityWebRequest.kHttpVerbGET;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if(!request.isNetworkError && request.responseCode == 200) {
                Debug.Log("Success");
                Debug.Log(request.downloadHandler.text);
            } else {
                Debug.Log(request.isNetworkError);
                Debug.Log(request.responseCode);
                Debug.Log("Fail");
            }
        }
        yield return 0;
    }

    private static IEnumerator PostRequest(string option, Dictionary<string, string> dic, bool isLogin = false) {
        string data = JsonConvert.SerializeObject(dic);
        using (UnityWebRequest request = UnityWebRequest.Post(Constants.SERVER_DOMAIN + option, data)) {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            yield return request.SendWebRequest();
            if(!request.isNetworkError && request.responseCode == 200) {
                Debug.Log(request.downloadHandler.data);
                if(isLogin) {
                    PlayerPrefs.SetString(Constants.USER, dic["username"]);
                    PanelManager.instance.AuthFalse();
                }
            } else {
                Debug.Log(request.isNetworkError);
                Debug.Log(request.responseCode);
                Debug.Log("Fail");
            }
        }
        yield return 0;
    }
}