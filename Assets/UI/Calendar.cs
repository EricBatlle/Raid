using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AssistanceSelector;
using static WebRequest;

[Serializable]
public class Calendar : MonoBehaviour
{
    [SerializeField] private GameObject dayPrefab = null;
    [Header("Day Information Panel")]
    [SerializeField] public DayInformation dayInfo = null;
    [Space()]
    [SerializeField] private GameObject monthLayout = null;    
    [SerializeField] private int totalMonthDays = 0;
    [SerializeField] private DateTime now = DateTime.Now;
    [SerializeField] public List<Day> month = new List<Day>();
    [SerializeField] public List<DayModel> monthModel = new List<DayModel>();
            
    public void CreateCalendar(Action<string> OnSuccess = null)
    {
        //Get how many days have this month
        totalMonthDays = DateTime.DaysInMonth(now.Year, now.Month);        
        //Create all empty Day Blocks before the first one which are days from another month
        for (int i = 0; i < DaysFromLastMonth(); i++)
        {
            GameObject newDay = Instantiate(dayPrefab);
            DayModel dayModel = new DayModel(false);
            newDay.GetComponent<Day>().dayModel = dayModel;
            newDay.transform.SetParent(monthLayout.transform);
        }
        //Create valid days
        foreach (DayModel dayModel in monthModel)
        {
            //Instantiate Day with DateTime info
            GameObject newDayObject = Instantiate(dayPrefab);
            Day dayComponent = newDayObject.GetComponent<Day>();
            dayModel.day = dayComponent;
            dayComponent.dayModel = dayModel;
            //dayComponent.GetAllAssistanceListsFromDB(()=> { });//dayComponent.acceptedRaiders = new List<Raider>(MainController.s_Instance.rosterController.raiders);
            dayComponent.SetDayNum();
            newDayObject.transform.SetParent(monthLayout.transform);
            month.Add(dayComponent);
        }
        GetAllDayAssistanceListsFromDB(OnSuccess, month);
    }

    public void GetAllDayAssistanceListsFromDB(Action<string> OnMultipleActionsEnd = null, List<Day> daysList = null)
    {
        int i = 0;
        foreach (Day day in daysList)
        {
            day.GetAllAssistanceListsFromDB((result) =>
            {
                i++;
                if (i == daysList.Count)                
                    OnMultipleActionsEnd(result);                
            });
        }
    }

    //Update dayModel on every day of the Month
    public void UpdateCalendarWithRaiderInformation()
    {
        for (int i = 0; i < month.Count; i++)
        {
            month[i].dayModel = MainController.s_Instance.currentRaider.month[i];
            MainController.s_Instance.currentRaider.month[i].day = month[i];
            month[i].RefreshUI();
        }
    }

    public List<DayModel> CreateCleanMonth()
    {
        monthModel = new List<DayModel>();

        //Get how many days have this month
        totalMonthDays = DateTime.DaysInMonth(now.Year, now.Month);
        for (int i = 0; i < totalMonthDays; i++)
        {
            DateTime dateTime = new DateTime(now.Year, now.Month, i + 1);
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            //Instantiate Day with DateTime info
            bool isRaideableDay = ((dayOfWeek == DayOfWeek.Saturday) || (dayOfWeek == DayOfWeek.Friday)) ? false : true;
            DayModel dayModel = new DayModel(null, i + 1, now.Month, now.Year, dayOfWeek, isRaideableDay, AssistanceType.Assist);
            monthModel.Add(dayModel);
        }

        return monthModel;
    }
    public void LoadMonthFromDBJson(string jsonInformation)
    {
        DayModel[] dayModelsDB = JsonManager.DeserializeFromJsonArray<DayModel>(jsonInformation);
        this.monthModel.Clear();
        foreach (DayModel day in dayModelsDB)
        {
            this.monthModel.Add(day);
        }
    }    

    //Search how many days before the first date of this month are from another month
    private int DaysFromLastMonth()
    {
        //Get first day of month
        DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;

        int daysFromLastMonth = (dayOfWeek == DayOfWeek.Sunday) ? 6 : (int)dayOfWeek - 1;
        return daysFromLastMonth;
    }
}
