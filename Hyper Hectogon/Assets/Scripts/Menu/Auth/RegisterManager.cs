using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] private InputField ID;
    [SerializeField] private InputField PW;
    [SerializeField] private InputField PWrepeat;
    [SerializeField] private Button RegisterButton;
    [SerializeField] private Button GoLoginButton;
    [SerializeField] private GameObject Login;
    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(Register);
        GoLoginButton.onClick.AddListener(() => {Login.SetActive(true);gameObject.SetActive(false);});
    }

    void Register() {
        if(PW.text == PWrepeat.text) {
            ServerManager.Register(this, ID.text, PW.text);
        } else {
            Debug.Log("Fail");
        }
    }
}
