using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using static AssistanceSelector;
using static WebRequest;

[System.Serializable]
public class Day : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Text dayNumText = null;
    [SerializeField] private Text lateInfoText = null;
    [SerializeField] private Text rotationInfoText = null;
    [Header("Model")]
    [SerializeField] public DayModel dayModel = null;
    [Space()]
    [SerializeField] public List<Raider> acceptedRaiders = new List<Raider>();
    [SerializeField] public List<Raider> lateRaiders = new List<Raider>();
    [SerializeField] public List<Raider> declinedRaiders = new List<Raider>();
    [SerializeField] public List<Raider> rosterRaiders = new List<Raider>();

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(OnButtonClick);        
    }
    private void Start()
    {
        RefreshUI();
    }

    int lastCount = 0;
    private void Update()
    {
        if (lateRaiders.Count != lastCount)
        {
            lastCount = lateRaiders.Count;
        }
    }

    //Set Color
    public void RefreshUI()
    {
        Image imageComp = this.gameObject.GetComponent<Image>();
        Button buttonComp = this.GetComponent<Button>();

        //Set color according to raideability        
        imageComp.color = (dayModel.IsRaideable) ? Color.white : Color.black;
        buttonComp.interactable = dayModel.IsRaideable;

        //SetColor according to assistance
        if(dayModel.IsRaideable)
        {
            switch (dayModel.AssistanceSelected)
            {
                case AssistanceType.Assist:
                    imageComp.color = Color.green;
                    lateInfoText.text = "";
                    break;
                case AssistanceType.Late:
                    imageComp.color = Color.yellow;                    
                    lateInfoText.text = LateTimeToString(dayModel.lateTime); //Set Text to see how Late
                    break;
                case AssistanceType.Decline:
                    imageComp.color = Color.red;
                    lateInfoText.text = "";
                    break;
            }
            
            //Update rotation infoText
            rotationInfoText.text = (dayModel.IsRotative) ? "R" : "";
            //Update rotation infoText with NOT ENOUGH PEOPLE
            if (rosterRaiders.Count < 20)                            
                rotationInfoText.text = (dayModel.IsRotative) ? "N" : "";            
        }        
    }

    private void OnButtonClick()
    {    
        UIController.s_Instance.OnDayClick(this);
    }

    #region DB
    private void LoadRaiderListFromDBJsonTo(List<Raider> raidersList, string jsonInformation)
    {
        Raider[] raidersListdDB = JsonManager.DeserializeFromJsonArray<Raider>(jsonInformation);
        raidersList.Clear();
        foreach (Raider raider in raidersListdDB)
        {
            raidersList.Add(raider);
        }
    }

    //GET ALL LISTS
    public void GetAllAssistanceListsFromDB(Action<string> OnSuccess = null)
    {
        DBController db = MainController.s_Instance.dbController;
        int thisDayID = this.dayModel.dayID;
        //Clear all lists
        this.CleanAllDayLists();

        db.PostGetDayAcceptedRaiders((resultAcceptedRaiders) =>
        {
            if (!WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultAcceptedRaiders))
                LoadRaiderListFromDBJsonTo(acceptedRaiders,resultAcceptedRaiders);

            db.PostGetDayDeclinedRaiders((resultDeclinedRaiders)=> 
            {
                if(!WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS,resultDeclinedRaiders))
                    LoadRaiderListFromDBJsonTo(declinedRaiders, resultDeclinedRaiders);

                db.PostGetDayLateRaiders((resultLateRaiders) =>
                {
                    if (!WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultLateRaiders))                    
                        LoadRaiderListFromDBJsonTo(lateRaiders, resultLateRaiders);

                    db.PostGetDayRosterRaiders((resultRosterRaiders) =>
                    {
                        if (!WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultRosterRaiders))
                            LoadRaiderListFromDBJsonTo(rosterRaiders, resultRosterRaiders);

                        OnSuccess?.Invoke(resultRosterRaiders);                        
                    }, thisDayID);
                }, thisDayID);
            }, thisDayID);
        }, thisDayID);
    }
    //REMOVE/ADD ABOVE LISTS
    private void RemoveRaiderFromListDB(Raider raiderToRemove, AssistanceType assistanceType)
    {
        switch (assistanceType)
        {
            case AssistanceType.Assist:
                MainController.s_Instance.dbController.PostRemoveRaiderOnAcceptedRaiders((result)=> { print(result); },raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.Decline:
                MainController.s_Instance.dbController.PostRemoveRaiderOnDeclinedRaiders((result) => { print(result); }, raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.Late:
                MainController.s_Instance.dbController.PostRemoveRaiderOnLateRaiders((result) => { print(result); }, raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.None:
                break;
        }
    }
    private void AddRaiderToListDB(Raider raiderToRemove, AssistanceType assistanceType)
    {
        switch (assistanceType)
        {
            case AssistanceType.Assist:
                MainController.s_Instance.dbController.PostAddRaiderOnAcceptedRaiders(null, raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.Decline:
                MainController.s_Instance.dbController.PostAddRaiderOnDeclinedRaiders(null, raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.Late:
                MainController.s_Instance.dbController.PostAddRaiderOnLateRaiders(null, raiderToRemove.raiderID, this.dayModel.dayID);
                break;
            case AssistanceType.None:
                break;
        }
    }    
    #endregion

    public string LateTimeToString(int lateTime)
    {
        int quartersLate =  lateTime/ 15;
        float auxHoursLate = quartersLate / 4;  //Needed to Math.Truncate function
        int hoursLate = (int)Math.Truncate(auxHoursLate);
        int remainingQuarters = (quartersLate - hoursLate * 4) * 15;        

        return hoursLate + "h" + remainingQuarters + "min";
    }

    public void SetDayNum()
    {
        this.dayNumText.text = dayModel.numDay.ToString();
    }

    public string ToDateString()
    {
        return this.dayModel.dayOfWeek.ToString() + " " + this.dayModel.numDay + "/" + this.dayModel.numMonth + "/" + this.dayModel.numYear;
    }

    public void UpdateDayAssistanceRaidersLists()
    {
        //Search in which list the current player belongs
        AssistanceType auxAssistanceType = SearchBelongingRaiderListType();
        //If it's the same list as previous, do nothing                
        if ((auxAssistanceType != dayModel.AssistanceSelected) && (auxAssistanceType != AssistanceType.None))
        {
            //If not, remove it from that list
            List<Raider> list = GetListFromAssistance(auxAssistanceType);
            Raider auxRaider = MainController.s_Instance.currentRaider;
            
            //Remove it locally
            list.Remove(GetRaiderFromList(list, auxRaider));    //Needs to be done like that, cause List.Remove needs THE item, not the copy                                                                
            //Remove it from DB
            RemoveRaiderFromListDB(auxRaider, auxAssistanceType);
            
            //Add it to the selected assitance type list
            GetListFromAssistance(dayModel.AssistanceSelected).Add(MainController.s_Instance.currentRaider);
            //Add it to DB
            AddRaiderToListDB(auxRaider, dayModel.AssistanceSelected);
        }
    }

    //Returns the Raider (not the copy) of the searched list with the same ID of the raider passed
    private Raider GetRaiderFromList(List<Raider> searchedList, Raider raider)
    {
        return searchedList.Find(Raider => Raider.raiderID == raider.raiderID);
    }

    //Search if the current raider belongs to the list
    private AssistanceType SearchBelongingRaiderListType()
    {
        //Check if the player is inside accepted list
        if ((acceptedRaiders.Find(Raider => Raider.raiderID == MainController.s_Instance.currentRaider.raiderID)) != null)        
            return AssistanceType.Assist;        
        else if ((lateRaiders.Find(Raider => Raider.raiderID == MainController.s_Instance.currentRaider.raiderID)) != null)        
            return AssistanceType.Late;        
        else if ((declinedRaiders.Find(Raider => Raider.raiderID == MainController.s_Instance.currentRaider.raiderID)) != null)        
            return AssistanceType.Decline;        
        else return AssistanceType.None;
    }

    //Return the raiders list depending on the assistance we want
    private List<Raider> GetListFromAssistance(AssistanceType assistanceType)
    {
        switch (assistanceType)
        {
            case AssistanceType.Assist:
                return acceptedRaiders;
            case AssistanceType.Decline:
                return declinedRaiders;
            case AssistanceType.Late:
                return lateRaiders;
            case AssistanceType.None:
                return null;
        }
        return null;
    }
    
    private void CleanAllDayLists()
    {
        acceptedRaiders.Clear();
        declinedRaiders.Clear();
        lateRaiders.Clear();
        rosterRaiders.Clear();
    }
}
