using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WebRequest;

public class DBController : Singleton<DBController>
{
    [SerializeField] private static string phpFilesDirectory = "D:/xampp/htdocs/myProjectorWhateverfoldername/";
    [SerializeField] private static string phpFilesServer = "http://rucker/";
    
    public void ResetDB()
    {
        this.GetCleanDatabase((resultCleanDB)=> 
        {
            this.PostNewCleanMonth((resultNewMonth)=> 
            {
                //this.GenerateNewCustomRoster((resultNewRoster)=> 
                //{
                    this.GetAllDatabaseInfo(()=> 
                    {
                        MainController.s_Instance.rosterController.GenerateRotations();
                        print("Clean Database: " + resultCleanDB +/* "\nGenerate New Roster: " + resultNewRoster +*/ "\nGenerate New Month: " + resultNewMonth);
                    });                                
                //});
            });
        });
    }

    public void GetAllDatabaseInfo(Action OnSuccess = null)
    {
        GetMonthDays((resultAllDays) =>
        {
            //ToDo: This is not needed to loaded from the DB isn't it? 
            //Well, it's necessary when more than one month is in game 
            //ToDo: Maybe auto-generated new months if empty?
            if (!WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultAllDays))                
                MainController.s_Instance.uiController.calendar.LoadMonthFromDBJson(resultAllDays);
            //Get All Raiders
            GetAllRaiders((resultAllRaiders) =>
            {
                if (WebResponse.isResultOk(resultAllRaiders))
                {
                    MainController.s_Instance.rosterController.LoadAllRaidersFromDBJson(resultAllRaiders);
                    GetMultipleRaidersMonthModel((result)=> { OnSuccess?.Invoke(); },MainController.s_Instance.rosterController.raiders);
                }
                else
                {
                    MainController.s_Instance.rosterController.raiders.Clear();
                    Debug.Log("ERROR GETTING ALL RAIDERS");
                    OnSuccess?.Invoke();
                }                
            });
        });               
    }

    #region Recursive Petitions
    #region GET
    public void GetMultipleRaidersMonthModel(Action<string> OnMultipleActionsEnd = null, List<Raider> raidersList = null)
    {
        bool isAllOk = true;
        int i = 0;
        foreach (Raider raider in raidersList)
        {
            this.PostGetRaiderMonthModel((result) =>
            {
                if (WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, result))
                {
                    isAllOk = false;
                    print("ERROR, THERE IS NO MONTH CREATED!");
                }
                else                
                    raider.LoadMonthFromDBJson(result);

                i++;
                if (i == raidersList.Count)
                {
                    string generalResult = (isAllOk) ? WebResponse.OK : WebResponse.ERROR;
                    OnMultipleActionsEnd(generalResult);
                }
            }, raider.raiderID);
        }
    }
    #endregion
    
    #region POST
    public void GenerateNewCustomRoster(Action<string> OnMultipleActionsEnd = null)
    {
        RosterController r = MainController.s_Instance.rosterController;
        r.CustomLoadRaiders();
        int i = 0;
        foreach (Raider raider in r.raiders)
        {
            this.PostNewRaider((result) =>
            {
                if (WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, result))
                    print("ERROR, THERE IS NO MONTH CREATED!");
                else
                    print(result);

                i++;
                if (i == r.raiders.Count)
                    OnMultipleActionsEnd(result);
            }, raider);
        }
    }

    public void MultiplePostsTest(Action<string> OnMultipleActionsEnd = null)
    {
        RosterController r = MainController.s_Instance.rosterController;
        r.CustomLoadRaiders();
        int i = 0;
        foreach (Raider raider in r.raiders)
        {
            this.PostNewRaider((result) =>
            {
                if (WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, result))
                    print("ERROR, THERE IS NO MONTH CREATED!");
                else
                    print(result);

                i++;
                if (i == r.raiders.Count)
                    OnMultipleActionsEnd(result);
            }, raider);
        }
    }
    #endregion
    #endregion

    #region GET
    public void GetAllRaiders(Action<string> OnSuccess = null)
    {
        string url = phpFilesServer + "GetAllRaiders.php";
        WebController.s_Instance.GetWebRequest(url, OnSuccess);
    }
    public void GetMonthDays(Action<string> OnSuccess = null)
    {
        string url = phpFilesServer + "GetAllMonthDays.php";
        WebController.s_Instance.GetWebRequest(url, OnSuccess);
    }
    public void GetCleanDatabase(Action<string> OnSuccess = null)
    {
        string url = phpFilesServer + "CleanDatabase.php";
        WebController.s_Instance.GetWebRequest(url, OnSuccess);
    }
    public void GetTest(Action<string> OnSuccess = null)
    {
        print("Using GetTest");
        //string url = phpFilesServer + "GetDayAcceptedRaiders.php";
        //WebController.s_Instance.GetWebRequest(url, (result) => { print(result); });
    }
    #endregion

    #region POST
    #region PostNew
    public void PostNewRaider(Action<string> OnSuccess = null, Raider newRaider = null)
    {
        string newRaiderString = JsonManager.SerializeToJson<Raider>(newRaider);
        string url = phpFilesServer + "GenerateNewRaider.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("newRaider",newRaiderString));
    }
    public void PostNewCleanMonth(Action<string> OnSuccess = null)
    {
        string monthString = JsonManager.SerializeToJsonArray(MainController.s_Instance.uiController.calendar.CreateCleanMonth().ToArray());
        string url = phpFilesServer + "GenerateMonthDays.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("newMonth",monthString));
    }
    #endregion
    public void PostLoginVerify(Action<string> OnSuccess = null, string newUsername = "defaultUsername", string newPassword = "defaultPassword")
    {
        string url = phpFilesServer + "LoginVerify.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("newRaiderID", newUsername), new WebRequest.FormField("newUsername", newUsername), new WebRequest.FormField("newPassword", newPassword));
    }
    public void PostGetRaiderMonthModel(Action<string> OnSuccess = null, int raiderID = 0)
    {
        string url = phpFilesServer + "GetRaiderMonthModel.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    #region Update
    public void PostUpdateDay(Action<string> OnSuccess = null, DayModel dayModelToUpdate = null, Raider newRaider = null)
    {
        //ToDo: This could be splited into more methods to only update the changed variable, not the whole dayModel
        string newDayModelString = JsonManager.SerializeToJson<DayModel>(dayModelToUpdate);
        string newRaiderString = JsonManager.SerializeToJson<Raider>(newRaider);
        string url = phpFilesServer + "UpdateDay.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("newDayModel", newDayModelString), new WebRequest.FormField("newRaider", newRaiderString));
    }
    public void PostUpdateRaider(Action<string> OnSuccess = null, Raider newRaider = null)
    {
        //ToDo: This could be splited into more methods to only update the changed variable, not the whole raider
        string newRaiderString = JsonManager.SerializeToJson<Raider>(newRaider);
        string url = phpFilesServer + "UpdateRaider.php";
        WebController.s_Instance.PostWebRequest(url, (result) => { print(result + "raiderName"+newRaider.name); }, new WebRequest.FormField("newRaider", newRaiderString));
    }
    #endregion
    #region DayLists_accepted_declined_late_roster
    #region GETLIST
    public void PostGetDayAcceptedRaiders(Action<string> OnSuccess = null, int dayID = 1)
    {
        string url = phpFilesServer + "GetDayAcceptedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()));
    }
    public void PostGetDayDeclinedRaiders(Action<string> OnSuccess = null, int dayID = 1)
    {
        string url = phpFilesServer + "GetDayDeclinedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()));
    }
    public void PostGetDayLateRaiders(Action<string> OnSuccess = null, int dayID = 1)
    {
        string url = phpFilesServer + "GetDayLateRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()));
    }
    public void PostGetDayRosterRaiders(Action<string> OnSuccess = null, int dayID = 1)
    {
        string url = phpFilesServer + "GetDayRosterRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()));
    }
    #endregion
    #region REMOVE
    public void PostRemoveRaiderOnAcceptedRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "RemoveRaiderOnAcceptedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostRemoveRaiderOnDeclinedRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "RemoveRaiderOnDeclinedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostRemoveRaiderOnLateRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "RemoveRaiderOnLateRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostRemoveRaiderOnRosterRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "RemoveRaiderOnRosterRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    #endregion
    #region ADD
    public void PostAddRaiderOnAcceptedRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "AddRaiderOnAcceptedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostAddRaiderOnDeclinedRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "AddRaiderOnDeclinedRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostAddRaiderOnLateRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "AddRaiderOnLateRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    public void PostAddRaiderOnRosterRaiders(Action<string> OnSuccess = null, int raiderID = 0, int dayID = 0)
    {
        string url = phpFilesServer + "AddRaiderOnRosterRaiders.php";
        WebController.s_Instance.PostWebRequest(url, OnSuccess, new WebRequest.FormField("dayID", dayID.ToString()), new WebRequest.FormField("raiderID", raiderID.ToString()));
    }
    #endregion
    #endregion

    public void PostTest(Action<string> OnSuccess = null, int dayID = 1)
    {

        print("Using PostTest");
        string url = phpFilesServer + "PostTest.php";
        WebController.s_Instance.PostWebRequest(url, (result)=> { print(result); }, new WebRequest.FormField("dayID", dayID.ToString()));
    }    
    #endregion    

}
