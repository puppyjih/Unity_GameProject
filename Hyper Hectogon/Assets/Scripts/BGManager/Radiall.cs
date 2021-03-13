using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radiall : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Text displayText;

    // Trackers for min/max values
    protected float maxValue = 2f, minValue = 0f;

    // Create a property to handle the slider's value
    private float currentValue = 0f;

    void Start()
    {
        //CurrentValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //CurrentValue += 0.0086f;
    }

    public void setText(string txt)
    {
        displayText.text = txt;
    }

}