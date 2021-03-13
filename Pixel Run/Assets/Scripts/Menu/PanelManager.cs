using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelManager : MonoBehaviour {
    // public variables
    public static PanelManager instance = null;

    // private variables
    [SerializeField]
    private GameObject[] _objects;
    [SerializeField]
    private GameObject[] _overlay_objects;
    [SerializeField]
    private Button HomeButton;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        if(instance == null) instance = this;
        HomeButton.onClick.AddListener(() => instance._changePanel("MainMenu"));
    }

    public void goHome() {
        _changePanel("MainMenu");
    }

    public void goRanking() {
        _changePanel("Ranking");
    }

    public void goSetting() {
        _changePanel("CharacterSelect");
    }

    public void PanelClose(string PanelName) {
        for(int i = 0; i < _objects.Length; i++) {
            if(_overlay_objects[i].name.Equals(PanelName)) {
                _overlay_objects[i].SetActive(false);
            }
        }
    }

    public void PanelOpen(string PanelName) {
        for(int i = 0; i < _overlay_objects.Length; i++) {
            if(_overlay_objects[i].name.Equals(PanelName)) {
                _overlay_objects[i].SetActive(true);
            }
        }
    }

    private void _changePanel(string activeName) {
        if(activeName.Equals("MainMenu")) {
            HomeButton.gameObject.SetActive(false);
        } else {
            HomeButton.gameObject.SetActive(true);
        }
        for(int i = 0; i < _objects.Length; i++) {
            if(_objects[i].name == activeName) {
                _objects[i].SetActive(true);
            } else {
                _objects[i].SetActive(false);
            }
        }
    }
}
