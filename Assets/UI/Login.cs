using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Header("Login Fields")]
    [SerializeField] private InputField usernameInput = null;
    [SerializeField] private InputField passwordInput = null;
    [Header("Display Information")]
    [SerializeField] private Text info_text = null;
    [Header("Buttons")]
    [SerializeField] private Button confirmButton = null;
    [SerializeField] private Button registerButton = null;

    private void Awake()
    {
        confirmButton.onClick.AddListener(()=> { MainController.s_Instance.uiController.OnConfirmLoginButtonClick(GetUsername(),GetPassword()); });
        registerButton.onClick.AddListener(MainController.s_Instance.uiController.OnGoRegisterButtonClick);
    }
    
    public string GetUsername()
    {
        return usernameInput.text;
    }
    private string GetPassword()
    {
        return passwordInput.text;
    }

    public void SetInformationPanel(string newInfo)
    {
        this.info_text.text = newInfo;
    }
}
