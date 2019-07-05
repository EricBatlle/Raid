using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayInformation : MonoBehaviour
{
    [SerializeField] private Text titleInformation = null;
    [SerializeField] private Button closeButton = null;
    [Header("Raiders Lists")]
    [SerializeField] private Text acceptedListTxt = null;
    [SerializeField] private Text lateListTxt = null;
    [SerializeField] private Text declinedListTxt = null;
    [Header("Rosters Lists")]
    [SerializeField] private Text tanksListTxt = null;
    [SerializeField] private Text healsListTxt = null;
    [SerializeField] private Text dpsListTxt = null;
    [Space()]
    [SerializeField] private Day selectedDay = null;
    [SerializeField]
    public Day SelectedDay
    {
        get
        {
            return selectedDay;
        }
        set //Not called if change property from editor
        {
            selectedDay = value;
            selectedDay.GetAllAssistanceListsFromDB((result)=> 
            {
                RefreshUI();
            });
        }
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(MainController.s_Instance.uiController.OnCloseDayInformationClick);
    }

    private void RefreshUI()
    {
        //Set Title
        titleInformation.text = selectedDay.ToDateString();
        //Set raiders lists 
        SetRaidersListsText();
        //Set roster lists
        SetRosterListsText();
    }

    private void SetRaidersListsText()
    {
        acceptedListTxt.text = "";
        lateListTxt.text = "";
        declinedListTxt.text = "";
        //Set list accepted Raiders
        for (int i = 0; i < selectedDay.acceptedRaiders.Count; i++)
        {
            acceptedListTxt.text += i + "\t" + selectedDay.acceptedRaiders[i].ToColorStringName(selectedDay.acceptedRaiders[i].mainClass) + '\n';
        }
        //Set late Raiders
        for (int i = 0; i < selectedDay.lateRaiders.Count; i++)
        {
            int newLateTime = selectedDay.lateRaiders[i].GetRaiderDayModel(selectedDay).lateTime;
            lateListTxt.text += i + "\t" + selectedDay.lateRaiders[i].ToColorStringName(selectedDay.lateRaiders[i].mainClass) + " " + selectedDay.LateTimeToString(newLateTime) + '\n';
        }
        //Set list Raiders
        for (int i = 0; i < selectedDay.declinedRaiders.Count; i++)
        {
            declinedListTxt.text += i + "\t" + selectedDay.declinedRaiders[i].ToColorStringName(selectedDay.declinedRaiders[i].mainClass) + '\n';
        }
    }

    private void SetRosterListsText()
    {
        tanksListTxt.text = "";
        healsListTxt.text = "";
        dpsListTxt.text = "";        
        foreach(Raider raider in selectedDay.rosterRaiders)
        {
            if(raider.mainSpec == Raider.Spec.Tank)            
                tanksListTxt.text += raider.ToColorStringName(raider.mainClass) + '\n';
            if(raider.mainSpec == Raider.Spec.Heal)            
                healsListTxt.text += raider.ToColorStringName(raider.mainClass) + '\n';
            if(raider.mainSpec == Raider.Spec.DPS)            
                dpsListTxt.text += raider.ToColorStringName(raider.mainClass) + '\n';            
        }
    }
}   
