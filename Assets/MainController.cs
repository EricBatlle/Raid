using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static AssistanceSelector;
using static Raider;
using static WebRequest;

public class MainController : Singleton<MainController>
{
    [SerializeField] private bool isAdmin = true;
    [Header("Current User")]
    [SerializeField] public Raider currentRaider = null;
    [Header("Controllers")]
    [SerializeField] public UIController uiController = null;
    [SerializeField] public RosterController rosterController = null;
    [SerializeField] public DBController dbController = null;
    [Header("Debug Information")]
    [SerializeField] private Text debugText = null;
    [Header("Color Classes")]
    [SerializeField] private Color warriorColor = Color.white;
    [SerializeField] private Color hunterColor = Color.white;
    [SerializeField] private Color priestColor = Color.white;
    [SerializeField] private Color shamanColor = Color.white;
    [SerializeField] private Color warlockColor = Color.white;
    [SerializeField] private Color druidColor = Color.white;
    [SerializeField] private Color paladinColor = Color.white;
    [SerializeField] private Color rogueColor = Color.white;
    [SerializeField] private Color deathKnightColor = Color.white;
    [SerializeField] private Color mageColor = Color.white;
    [SerializeField] private Color monkColor = Color.white;
    [SerializeField] private Color demonHunterColor = Color.white;

    public Dictionary<Class, Color> ColorClassDictionary = null;

    private void Awake()
    {
        //Build non-static Dictionary
        ColorClassDictionary = new Dictionary<Class, Color>
        {
            { Class.Warrior, warriorColor},
            { Class.Hunter, hunterColor},
            { Class.Priest, priestColor},
            { Class.Shaman, shamanColor},
            { Class.Warlock, warlockColor},
            { Class.Druid, druidColor},
            { Class.Paladin, paladinColor},
            { Class.Rogue, rogueColor},
            { Class.DeathKnight, deathKnightColor},
            { Class.Mage, mageColor},
            { Class.Monk, monkColor},
            { Class.DemonHunter, demonHunterColor}
        };
        
        //Controllers Reference
        uiController = UIController.s_Instance;
        rosterController = RosterController.s_Instance;
        dbController = DBController.s_Instance;

        //Get DB information       
        dbController.GetAllDatabaseInfo(()=> 
        {
            print("Ended GetAllDatabaseInfo");
            ////Build UI
            uiController.StartUI((result)=> 
            {
                print("Ended Start UI");
                //Generate Roster/ Rotations
                rosterController.GenerateRotations();
            });
        });        
    }

    public void ChangeCurrentRaider(string loginUsername)
    {
        currentRaider = rosterController.GetRaiderFromUsername(loginUsername);
        //Update currentRaiderMonthModel
        dbController.PostGetRaiderMonthModel((resultGetRaiderMonthModel) =>
        {
            if (WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultGetRaiderMonthModel))
            {
                //ToDo: WHY THE HELL I'M GETTING THIS WHEN NEW ROSTER IS CREATED?!
                print("ERROR, THERE IS NO MONTH CREATED!");
                return;
            }
            currentRaider.LoadMonthFromDBJson(resultGetRaiderMonthModel);
            uiController.calendar.UpdateCalendarWithRaiderInformation();

            uiController.CloseLogin();
            uiController.OpenCalendar();
        }, currentRaider.raiderID);
    }

    #region DEBUG
    private void Update()
    {
        debugText.text = currentRaider.ToDebugString();        
    }
    //set all raiders with the new calendar, 
    public void SetInitialCalendarToRaiders()
    {
        foreach (Raider raider in rosterController.raiders)
        {
            raider.month = raider.ToDayModelList(uiController.calendar.month).ToList();
        }
    }
    public void ChangeCurrentRaider(int raiderID)
    {
        currentRaider = rosterController.GetRaiderFromID(raiderID);
        //Update currentRaiderMonthModel
        dbController.PostGetRaiderMonthModel((resultGetRaiderMonthModel) =>
        {
            if (WebResponse.isEqualTo(WebResponse.ERROR_0RESULTS, resultGetRaiderMonthModel))
            {
                print("ERROR, THERE IS NO MONTH CREATED!");
                return;
            }
            currentRaider.LoadMonthFromDBJson(resultGetRaiderMonthModel);
            uiController.calendar.UpdateCalendarWithRaiderInformation();

            uiController.CloseLogin();
            uiController.OpenCalendar();
        }, currentRaider.raiderID);
    }
    #endregion

}
