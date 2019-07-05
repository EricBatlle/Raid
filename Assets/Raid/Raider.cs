using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Raider
{

    public enum Spec { Tank, Heal, DPS, None };
    public enum Class { Warrior, Hunter, Priest, Shaman, Warlock, Druid, Paladin, Rogue, DeathKnight, Mage, Monk, DemonHunter };    

    [SerializeField] public int raiderID = 0;
    [SerializeField] public string username = "defaultUsername";    
    [SerializeField] public string password = "defaultPassword";    
    [SerializeField] public string name = "defaultName";
    [SerializeField] public Class mainClass = Class.Warrior;
    [SerializeField] public Spec mainSpec = Spec.DPS;
    [SerializeField] private Spec offSpec = Spec.None;
    [Space()]
     public List<DayModel> month = new List<DayModel>();
    [SerializeField] public int daysRotated = 0;
    
    #region Constructors
    public Raider(int ID = 0, string username = "defaultUsername", string password="defaultPassword", string name = "defaultName", Spec mainSpec = Spec.DPS, Spec offSpec = Spec.None, Class mainClass = Class.Warrior)
    {
        this.raiderID = ID;
        this.username = username;
        this.password = password;
        this.name = name;
        this.mainSpec = mainSpec;
        this.offSpec = offSpec;
        this.mainClass = mainClass;
        month = ToDayModelList(MainController.s_Instance.uiController.calendar.month).ToList();
        this.daysRotated = 0;
    }
    public Raider(int ID = 0, string name = "defaultName", Spec mainSpec = Spec.DPS, Spec offSpec = Spec.None, Class mainClass = Class.Warrior )
    {
        this.raiderID = ID;
        this.name = name;
        this.mainSpec = mainSpec;
        this.offSpec = offSpec;
        this.mainClass = mainClass;
        month = ToDayModelList(MainController.s_Instance.uiController.calendar.month).ToList();
        this.daysRotated = 0;
    }
    public Raider(int ID = 0, Spec mainSpec = Spec.DPS)
    {
        this.raiderID = ID;
        this.name = "defaultName";
        this.mainSpec = mainSpec;
        this.offSpec = Spec.None;
        this.mainClass = Class.Warrior;
        month = ToDayModelList(MainController.s_Instance.uiController.calendar.month).ToList();
        this.daysRotated = 0;
    }
    public Raider(Spec mainSpec = Spec.DPS)
    {
        this.raiderID = 0;
        this.name = "defaultName";
        this.mainSpec = mainSpec;
        this.offSpec = Spec.None;
        this.mainClass = Class.Warrior;
        month = ToDayModelList(MainController.s_Instance.uiController.calendar.month).ToList();
        this.daysRotated = 0;
    }
    public Raider(int ID = 0)
    {
        this.raiderID = ID;
        this.name = "defaultName";
        this.mainSpec = Spec.DPS;
        this.offSpec = Spec.None;
        this.mainClass = Class.Warrior;
        month = ToDayModelList(MainController.s_Instance.uiController.calendar.month).ToList();
        this.daysRotated = 0;
    }

    public List<DayModel> ToDayModelList(List<Day> dayList)
    {
        List<DayModel> auxList = new List<DayModel>();
        foreach (Day day in dayList)
        {
            auxList.Add(new DayModel(day.dayModel));
        }
        return auxList;
    }
    #endregion

    public DayModel GetRaiderDayModel(Day day)
    {
        return month.Find(DayModel => DayModel.numDay == day.dayModel.numDay);
    }
    public DayModel GetRaiderDay(int dayID)
    {
        return month.Find(DayModel => DayModel.dayID == dayID);
    }

    #region DB
    public void LoadMonthFromDBJson(string jsonInformation)
    {
        DayModel[] dayModelsDB = JsonManager.DeserializeFromJsonArray<DayModel>(jsonInformation);
        this.month.Clear();
        //Here can be made the link between ModelDay and Day, but instead we deliver that job to calendar UpdateCalendarWithRaiderInformation
        foreach (DayModel day in dayModelsDB)
        {
            this.month.Add(day);
        }
    }
    public static Raider LoadRaiderFromDBJson(string jsonInformation)
    {
        Debug.Log(jsonInformation);
        Raider newRaider = JsonManager.DeserializeFromJson<Raider>(jsonInformation);
        return newRaider;
    }
    #endregion
    
    #region To
    public string ToDebugString()
    {
        return "ID: " + this.raiderID + "\n Name: " + this.name + "\n Class: "+ ToColorStringClass(this.mainClass) +"\n MainSpec: " + this.mainSpec + "\n Days Rotated: "+ this.daysRotated;
    }
    
    private static string ToColorStringClass(Class newClass)
    {
        return " <color=#" + ColorUtility.ToHtmlStringRGBA(MainController.s_Instance.ColorClassDictionary[newClass]) + ">" + newClass + "</color>";
    }
    public string ToColorStringName(Class newClass)
    {
        return " <color=#" + ColorUtility.ToHtmlStringRGBA(MainController.s_Instance.ColorClassDictionary[this.mainClass]) + ">" + this.name + "</color>";
    }
    #endregion
}
