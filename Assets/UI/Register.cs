using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Raider;

public class Register : MonoBehaviour
{
    [Header("Register Fields")]
    [SerializeField] private InputField usernameInput = null;
    [SerializeField] private InputField passwordInput = null;
    [SerializeField] private InputField raiderNameInput = null;
    [SerializeField] private Dropdown classDropdown = null;
    [SerializeField] private Dropdown mainSpecDropdown = null;
    [SerializeField] private Dropdown offSpecDropdown = null;

    [Header("Display Information")]
    [SerializeField] private Text info_text = null;

    [Header("Buttons")]
    [SerializeField] private Button confirmRegisterButton = null;
    [SerializeField] private Button backButton = null;

    private void Awake()
    {
        confirmRegisterButton.onClick.AddListener(()=> { MainController.s_Instance.uiController.OnConfirmRegisterButtonClick(GetRegisterRaider()); });
        backButton.onClick.AddListener(MainController.s_Instance.uiController.OnGoBackButtonClick);
    }

    private Raider GetRegisterRaider()
    {
        int id = 0;
        string username = usernameInput.text;
        string password = passwordInput.text;
        string name = raiderNameInput.text;
        Class mainClass = (Class)classDropdown.value;
        Spec mainSpec = (Spec)mainSpecDropdown.value;
        Spec offSpec = (Spec)offSpecDropdown.value;
        Raider raider = new Raider(id, name, mainSpec, offSpec, mainClass);
        raider.username = username;
        raider.password = password;

        return raider;
    }

    public void SetInformationPanel(string newInfo)
    {
        this.info_text.text = newInfo;
    }
}
