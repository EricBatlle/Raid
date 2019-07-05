using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RosterController : Singleton<RosterController>
{
    [SerializeField] public List<Raider> raiders = new List<Raider>();
    [SerializeField] public List<Raider> auxRaidersAssistToday = new List<Raider>();
    [SerializeField] public List<Raider> orderedAcceptedList = new List<Raider>();
    [Header("Roster Size")]
    [SerializeField] private int neededTanks = 2;
    [SerializeField] private int neededHeals = 5;
    [SerializeField] private int neededDps = 13;

    private Raider[] raidersDB;    

    public void UpdateRaidersList()
    {
        MainController.s_Instance.dbController.GetAllRaiders((result)=> 
        {
            LoadAllRaidersFromDBJson(result);
        });
    }

    #region LoadRaiders
    public void LoadAllRaidersFromDBJson(string jsonInformation)
    {        
        raidersDB = JsonManager.DeserializeFromJsonArray<Raider>(jsonInformation);
        foreach (Raider raider in raidersDB)
        {
            RegisterNewRaider(raider);
        }
    }

    #region DEBUG
    public void CustomLoadRaiders()
    {        
        int i = 0;
        int u = 0;
        int t = 0;
        //Create 3 Tanks
        for (i = 0; i < 3; i++)
        {
            Raider raider = new Raider(i, Raider.Spec.Tank);
            raider.raiderID = i + 1;
            raider.name = "t" + i;
            raider.username = "t" + i;
            raider.password = "t" + i;
            raiders.Add(raider);
        }
        //Create 6 Healers
        for (u = 0 + i; u < 6 + i; u++)
        {
            Raider raider = new Raider(u, Raider.Spec.Heal);
            raider.raiderID = u + 1;
            raider.name = "h" + u;
            raider.username = "h" + u;
            raider.password = "h" + u;
            raiders.Add(raider);
        }
        //Create 18 DPS
        for (t = 0+u; t < 18+u; t++)
        {
            Raider raider = new Raider(t, Raider.Spec.DPS);
            raider.raiderID = t + 1;
            raider.name = "d" + t;
            raider.username = "d" + t;
            raider.password = "d" + t;
            raiders.Add(raider);
        }
    }
    #endregion

    #endregion

    #region GetRaiderFrom
    public Raider GetRaiderFromUsername(string usernameToFind)
    {
        return raiders.Find(Raider => Raider.username == usernameToFind);
    }
    public Raider GetRaiderFromID(int id)
    {
        return raiders.Find(Raider => Raider.raiderID == id);
    }
    #endregion

    #region Register
    private void RegisterNewRaider(Raider newRaider)
    {
        this.raiders.Add(newRaider);
    }    
    public void OnPostNewRaider()
    {
        //ToDo: Is this really needed?
        //newRaider.month = MainController.s_Instance.uiController.calendar.monthModel;
        UpdateRaidersList();
    }
    #endregion

    #region CalculateRotations
    /*
     * Generate rotations based on:
     *  1-Assistance
     *  2-Days Rotated
     *  3-People needed
     *  ¿4?-Items needed on that boss (needs to implement 2 much stuff at the moment)
     *  
     */
    public void GenerateDayRotation(Day day)
    {
        DBController db = MainController.s_Instance.dbController;

        if (day.dayModel.IsRaideable && day.acceptedRaiders.Count > 0)
        {            
            day.rosterRaiders.Clear(); //To avoid overlaping
            //Create roster
            //1-Assistance
            auxRaidersAssistToday = day.acceptedRaiders.ToList();
            //2-Days Rotated
            //Order the list by descending, so the people who rotate the most is on top of the list, that save the triple list needed to order tanks, heals and dps's by score
            orderedAcceptedList = day.acceptedRaiders.OrderByDescending(raider => raider.daysRotated).ToList();
            //3-People needed
            int numTanks = 0;
            int numHeals = 0;
            int numDps = 0;            

            foreach (Raider raider in orderedAcceptedList)
            {
                if ((raider.mainSpec == Raider.Spec.Tank) && numTanks < neededTanks)
                {
                    day.rosterRaiders.Add(raider);
                    //ToDo: Make DB adds!
                    auxRaidersAssistToday.Remove(raider);
                    numTanks++;
                }
                if ((raider.mainSpec == Raider.Spec.Heal) && numHeals < neededHeals)
                {
                    day.rosterRaiders.Add(raider);
                    auxRaidersAssistToday.Remove(raider);
                    numHeals++;
                }
                if ((raider.mainSpec == Raider.Spec.DPS) && numDps < neededDps)
                {
                    day.rosterRaiders.Add(raider);
                    auxRaidersAssistToday.Remove(raider);
                    numDps++;
                }
            }
            //Check if the roster is completed
            if (day.rosterRaiders.Count < 20)
            {
                string errorMessage = "ERROR: NOT ENOUGH DPS TO SET THE ROSTER ON " + day.ToDateString();
                errorMessage += "\nnumTanks " + numTanks + "numHeals " + numHeals + " numDps " + numDps;
                print(errorMessage);
                day.RefreshUI();
            }
            else
            {
                day.acceptedRaiders.Clear();
                //Update rotated days on EXCLUDED raiders
                foreach (Raider raider in auxRaidersAssistToday)
                {
                    Raider raiderFinded = raiders.Find(Raider => Raider.raiderID == raider.raiderID);
                    print("day " + day.dayModel.dayID + "from " + raider.name);
                    print("raider"+ raiders.Find(Raider => Raider.raiderID == raider.raiderID).name);
                    print("dayModel"+ raiders.Find(Raider => Raider.raiderID == raider.raiderID).GetRaiderDay(day.dayModel.dayID));
                    DayModel dayModelFinded = raiders.Find(Raider => Raider.raiderID == raider.raiderID).GetRaiderDay(day.dayModel.dayID);
                    //Update Raider from raiders
                    //raiders.Find(Raider => Raider.raiderID == raider.raiderID).daysRotated++;
                    raiderFinded.daysRotated++;
                    //db.PostUpdateRaider(null, raiderFinded);
                    //Update Day
                    //raiders.Find(Raider => Raider.raiderID == raider.raiderID).GetRaiderDay(day.dayModel.dayID).IsRotative = true;
                    dayModelFinded.IsRotative = true;
                    //db.PostUpdateDay(null, dayModelFinded, raiderFinded); 
                    if(day.acceptedRaiders.Count > 0)
                        day.acceptedRaiders.Find(Raider => Raider.raiderID == raiderFinded.raiderID).daysRotated++;
                }
                //Update rotated days and information on INCLUDED raiders
                foreach (Raider raider in day.rosterRaiders)
                {
                    Raider raiderFinded = raiders.Find(Raider => Raider.raiderID == raider.raiderID);
                    DayModel dayModelFinded = raiders.Find(Raider => Raider.raiderID == raider.raiderID).GetRaiderDay(day.dayModel.dayID);
                    //ToDo: Should rest daysRotated to avoid declined accident click?
                    dayModelFinded.IsRotative = false;
                    //db.PostUpdateDay(null, dayModelFinded, raiderFinded);
                }
            }
        }
    }
    public void GenerateRotations()
    {
        //2 tanks + 5 heals + 13 DPS
        foreach (Day day in MainController.s_Instance.uiController.calendar.month)
        {
            GenerateDayRotation(day);
        }
    }
    #endregion
}
