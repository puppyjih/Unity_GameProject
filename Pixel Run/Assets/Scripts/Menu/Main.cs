using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    // private variables
    [SerializeField]
    private Button GamePlayBtn, HighScoreBtn, CharacterBtn;

    // Start is called before the first frame update
    void Start()
    {
        //GamePlayBtn.onClick.AddListener(() => Scene.goScene("GamePlay"));
        HighScoreBtn.onClick.AddListener(PanelManager.instance.goRanking);
        CharacterBtn.onClick.AddListener(PanelManager.instance.goSetting);
    }
}
