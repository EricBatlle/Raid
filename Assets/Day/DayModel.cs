using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AssistanceSelector;

[Serializable]
public class DayModel
{
    public Day day;

    [Header("Information")]
    [SerializeField] public int raiderID = 0;
    [SerializeField] public int dayID = 0;
    [SerializeField] public int numDay = 0;
    [SerializeField] public int numMonth = 0;
    [SerializeField] public int numYear = 0;
    [SerializeField] public DayOfWeek dayOfWeek = DayOfWeek.Monday;
    [SerializeField] private bool isRaideable = true;   //If it is a valid day to raid: it's not friday or saturday, or another month day
    public bool IsRaideable
    {
        get
        {
            return isRaideable;
        }
        set //Not called if change property from editor
        {
            isRaideable = value;
            day?.RefreshUI();
            MainController.s_Instance.dbController.PostUpdateDay(null, this, MainController.s_Instance.rosterController.GetRaiderFromID(raiderID));
        }
    }
    [SerializeField] private bool isRotative = false;   
    public bool IsRotative
    {
        get
        {
            return isRotative;
        }
        set //Not called if change property from editor
        {
            isRotative = value;
            day?.RefreshUI(); //ToDo: THIS COULD THROW NULL ERROR CAUSE THE LINK BETWEEN DAY AND DAYMODEL IS NOT MADE IT FOR EVERY RAIDER!
            //ToDo: Should not be Current raider, what happens if we update one day by rotations to another raider?
            MainController.s_Instance.dbController.PostUpdateDay(null, this, MainController.s_Instance.rosterController.GetRaiderFromID(raiderID));
        }
    }
    [SerializeField] private AssistanceType assistanceSelected = AssistanceType.Assist;    
    public AssistanceType AssistanceSelected
    {
        get
        {
            return assistanceSelected;
        }
        set //Not called if change property from editor
        {
            assistanceSelected = value;
            day.RefreshUI();
            day.UpdateDayAssistanceRaidersLists();
            MainController.s_Instance.dbController.PostUpdateDay(null,this, MainController.s_Instance.rosterController.GetRaiderFromID(raiderID));
        }
    }
    public int lateTime = 0;

    #region Constructors
    public DayModel(Day day, int numDay = 1, int numMonth = 1, int numYear = 1995, DayOfWeek dayOfWeek = DayOfWeek.Monday, bool isRaideable = true, AssistanceType assistanceSelected = AssistanceType.Assist)
    {
        this.dayID = 0;
        this.day = day;
        this.numDay = numDay;
        this.numMonth = numMonth;
        this.numYear = numYear;
        this.dayOfWeek = dayOfWeek;
        this.isRaideable = isRaideable;
        this.assistanceSelected = assistanceSelected;
    }
    public DayModel(bool isRaideable)
    {
        this.dayID = 0;
        this.numDay = 1;
        this.numMonth = 1;
        this.numYear = 1995;
        this.dayOfWeek = DayOfWeek.Monday;
        this.isRaideable = isRaideable;
        this.assistanceSelected = AssistanceType.None;
    }
    public DayModel(DayModel newDayModel)
    {
        this.dayID = 0;
        this.day = newDayModel.day;
        this.numDay = newDayModel.numDay;
        this.numMonth = newDayModel.numMonth;
        this.numYear = newDayModel.numYear;
        this.dayOfWeek = newDayModel.dayOfWeek;
        this.isRaideable = newDayModel.isRaideable;
        this.assistanceSelected = newDayModel.assistanceSelected;
    }
    #endregion
}
