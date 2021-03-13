using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    // private variables
    [SerializeField]
    private Button _GamePlayBtn, _HighScoreBtn, _CharacterBtn;

    // Start is called before the first frame update
    void Start() {
        _GamePlayBtn.onClick.AddListener(_goClassicScene);
        _HighScoreBtn.onClick.AddListener(PanelManager.instance.goHighScore);
        _CharacterBtn.onClick.AddListener(PanelManager.instance.goSetting);
    }
    
    private void _goClassicScene() {
        Scene.goScene("Classic");
    }
}
