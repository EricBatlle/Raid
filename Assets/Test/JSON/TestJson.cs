using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TestJson : Singleton<TestJson>
{
    [SerializeField] private JsonClass jsonClass = null;
    [SerializeField] private List<JsonClass> jsonClassList = new List<JsonClass>();
    [SerializeField] private Text jsonText = null;
    // Start is called before the first frame update
    private void Start()
    {
        WebController.s_Instance.GetWebRequest("https://jsonplaceholder.typicode.com/todos/1", (result) =>
        {
            jsonClass = JsonManager.DeserializeFromJson<JsonClass>(result);
            jsonText.text = JsonManager.SerializeToJson<JsonClass>(jsonClass);
            ModifyRaiders();
        });
        //GetAllRaiders();
        //ModifyRaiders();
    }

    public string GetFileJson()
    {
        string path = "Assets/Resources/jsonTest.txt";
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();        
    }
    public void GetAllRaiders()
    {
        JsonClass[] jsonClasses = JsonManager.DeserializeFromJsonArray<JsonClass>(GetFileJson());
        foreach(JsonClass json in jsonClasses)
        {
            jsonClassList.Add(json);
        }
    }

    public void ModifyRaiders()
    {
        jsonClass.userId = 54;
    }
}
