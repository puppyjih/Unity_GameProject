using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageView : MonoBehaviour
{
    public static StageView instance = null;
    [SerializeField] private Text stageText;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
        
    }

    public void View(int mainStage, int subStage) {
        StartCoroutine(StartView(mainStage.ToString() + "-" + subStage.ToString()));
    }

    public void View(string msg) {
        StartCoroutine(StartView(msg));
    }

    private IEnumerator StartView(string msg) {
        stageText.text = msg;
        yield return new WaitForSeconds(1f);
        stageText.text = "";
    }
}
