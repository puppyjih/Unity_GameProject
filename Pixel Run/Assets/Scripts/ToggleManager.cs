using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Toggle toggle;
    public GameObject weapon;

    private void Awake()
    {
        Inventory inventory = transform.Find("Player").GetComponent<Inventory>();
        toggle.onValueChanged.AddListener((isOn) =>
        {
            Image cb = toggle.targetGraphic.GetComponent<Image>();
            if (isOn)
            {
                Debug.Log("hi");
                cb.color = new Color(77 / 255f, 255 / 255f, 221 / 255f, 2000 / 255f);

                //cb.highlightedColor = new Color(0, 255, 221, 255);
            }
            else
            {
                Debug.Log("Bye");
                cb.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 141 / 255f);
                //cb.highlightedColor = new Color(255, 255, 255, 255);
            }
        });
    }
}
