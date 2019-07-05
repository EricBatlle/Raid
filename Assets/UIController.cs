using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AssistanceSelector;
using static WebRequest;

public class UIController : Singleton<UIController>
{
    [Header("Login")]
    [SerializeField] private Login login = null;

    [Header("Register")]
    [SerializeField] private Register register = null;

    [Header("Calendar")]
    public Calendar calendar = null;
    private Day selectedDay = null;    

    

    public void StartUI(Action<string> OnSuccess = null)
    {
        calendar.CreateCalendar(OnSuccess);

        CloseCalendar();
        CloseRegister();
        OpenLogin();
    }    

    #region OpenClose UI components
    //CALENDAR
    public void OpenCalendar()
    {
        CloseDayInfo();
        calendar.gameObject.SetActive(true);
    }    
    public void CloseCalendar()
    {
        CloseDayInfo();
        calendar.gameObject.SetActive(false);
    }
    //DAY_INFO
    public void OpenDayInfo()
    {
        calendar.dayInfo.gameObject.SetActive(true);
    }
    public void CloseDayInfo()
    {
        calendar.dayInfo.gameObject.SetActive(false);
    }
    //LOGIN
    public void OpenLogin()
    {
        login.gameObject.SetActive(true);
    }    
    public void CloseLogin()
    {
        login.gameObject.SetActive(false);
    }
    //REGISTER
    public void OpenRegister()
    {
        register.gameObject.SetActive(true);
    }    
    public void CloseRegister()
    {
        register.gameObject.SetActive(false);
    }    
    #endregion

    #region OnClickUIEventsNavigation

    #region Calendar
    public void OnDayClick(Day dayClicked)
    {       
        selectedDay = dayClicked;
        //Open DayInfo
        calendar.dayInfo.SelectedDay = selectedDay;  //ToDo: Check if the order affects in some way
        OpenDayInfo();
    }
    #endregion

    #region DayInformation Panel
    public void OnAssistanceTypeClick(AssistanceType assistanceType, int lateTime = 0)
    {        
        CloseDayInfo();
        //Set Day assistance (To change style)
        selectedDay.dayModel.lateTime = lateTime;
        selectedDay.dayModel.AssistanceSelected = assistanceType;
        //Update current Raider Calendar        
        MainController.s_Instance.currentRaider.month[MainController.s_Instance.currentRaider.month.FindIndex(DayModel => DayModel == selectedDay.dayModel)] = selectedDay.dayModel;
        //Update Rotations
        //MainController.s_Instance.rosterController.GenerateDayRotation(selectedDay);
        //UpdateDB is done in the properties, as it can be affected not only by assistance        
    }
    public void OnCloseDayInformationClick()
    {
        CloseDayInfo();
    }
    #endregion

    #region LOGIN
    public void OnConfirmLoginButtonClick(string username, string password)
    {
        MainController.s_Instance.dbController.PostLoginVerify((resultLoginVerify) =>
        {
            if (VerifyLogin(resultLoginVerify))
            {
                //Get the login raider and set it as the current raider for this season
                string loginUsername = MainController.s_Instance.uiController.login.GetUsername();
                MainController.s_Instance.ChangeCurrentRaider(loginUsername);                
            }
        }, username, password);
    }
    public void OnGoRegisterButtonClick()
    {
        CloseLogin();
        OpenRegister();
    }
    public bool VerifyLogin(string result)
    {
        if (WebResponse.isResultOk(result))
        {
            MainController.s_Instance.uiController.login.SetInformationPanel("");
            return true;
        }
        else if (WebResponse.isEqualTo(WebResponse.ERROR_LOGIN_UNEXISTANT_USERNAME, result))        
            MainController.s_Instance.uiController.login.SetInformationPanel("This user does not exist");        
        else if(WebResponse.isEqualTo(WebResponse.ERROR_LOGIN_WRONG_CREDENTIALS,result))        
            MainController.s_Instance.uiController.login.SetInformationPanel("Wrong credentials");

        return false;
    }
    #endregion

    #region REGISTER
    public void OnConfirmRegisterButtonClick(Raider raider)
    {        
        MainController.s_Instance.dbController.PostNewRaider((result)=> 
        {
            if(VerifyRegister(result))
            {
                CloseRegister();
                MainController.s_Instance.rosterController.OnPostNewRaider();
                OpenLogin();
            }
        }, raider);
    }
    public void OnGoBackButtonClick()
    {
        CloseRegister();
        OpenLogin();
    }
    public bool VerifyRegister(string result)
    {
        if (WebResponse.isResultOk(result))
        {
            MainController.s_Instance.uiController.login.SetInformationPanel("");
            return true;
        }
        else if (WebResponse.isEqualTo(WebResponse.ERROR_REGISTER_DUPLICATE_USERNAME, result))
            MainController.s_Instance.uiController.register.SetInformationPanel("This username is already taken");
        else
            MainController.s_Instance.uiController.register.SetInformationPanel("Error on register");

        return false;
    }
    #endregion

    #endregion
}
