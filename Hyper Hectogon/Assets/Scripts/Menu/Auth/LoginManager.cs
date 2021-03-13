using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private InputField ID;
    [SerializeField] private InputField PW;
    [SerializeField] private Button LoginButton;
    [SerializeField] private Button GoRegisterButton;
    [SerializeField] private GameObject Register;
    // Start is called before the first frame update
    void Start()
    {
        LoginButton.onClick.AddListener(Login);
        GoRegisterButton.onClick.AddListener(() => {Register.SetActive(true);gameObject.SetActive(false);});
    }

    private void Update() {
    }

    void Login() {
        ServerManager.Login(this, ID.text, PW.text);
    }
}
