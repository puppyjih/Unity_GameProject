using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    // public variables
    public static PanelManager instance = null;

    // private variables
    [SerializeField]
    private GameObject[] _objects;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        if(instance == null) instance = this;
    }

    public void goHome() {
        _changePanel("MainMenu");
    }

    public void goHighScore() {
        _changePanel("HighScore");
    }

    public void goSetting() {
        _changePanel("CharacterSelect");
    }

    public void AuthFalse() {
        for(int i = 0; i < _objects.Length; i++) {
            if(_objects[i].name == "Auth") {
                _objects[i].SetActive(false);
            }
        }
    }

    private void _changePanel(string activeName) {
        for(int i = 0; i < _objects.Length; i++) {
            if(_objects[i].name == activeName) {
                _objects[i].SetActive(true);
            } else {
                _objects[i].SetActive(false);
            }
        }
    }
}
