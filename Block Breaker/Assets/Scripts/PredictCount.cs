using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PredictCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    private int guessCnt;
    void Start()
    {
        //guessCnt = PlayerPrefs.GetInt("guessCnt");
        //tmp.text = guessCnt.ToString();
        //PlayerPrefs.Save();
    }
}
