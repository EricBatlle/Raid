using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Raider;
using static WebRequest;

[CustomEditor(typeof(DBController))]
public class DBControllerEditor : Editor
{
    [Header("Post New Raider")]
    [SerializeField] public string username = "defaultUserName";
    [SerializeField] public string password = "defaultPassword";
    [SerializeField] public string raiderName = "defaultName";
    [SerializeField] public Class mainClass = Class.Warrior;
    [SerializeField] public Spec mainSpec = Spec.DPS;
    [SerializeField] private Spec offSpec = Spec.None;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DBController dbController = (DBController)target;

        #region Database functionalities 
        //Post New Raider
        username = EditorGUILayout.TextField("Username:", username);
        password = EditorGUILayout.TextField("Password:", password);
        raiderName = EditorGUILayout.TextField("Name:", raiderName);
        mainClass = (Class)EditorGUILayout.EnumPopup("Main Class:",mainClass);
        mainSpec = (Spec)EditorGUILayout.EnumPopup("Main Spec:", mainSpec);
        offSpec = (Spec)EditorGUILayout.EnumPopup("Off Spec:", offSpec);
        if (GUILayout.Button("Post New Raider"))
        {
            Raider newRaider = new Raider(0, username, password, raiderName, mainSpec, offSpec, mainClass);
            dbController.PostNewRaider((result)=> 
            {
                if (WebResponse.isResultOk(result))
                    MainController.s_Instance.rosterController.OnPostNewRaider();                

                Debug.Log("Post New Raider: "+result);
            },newRaider);
        }
        if (GUILayout.Button("Post New Roster"))
            dbController.GenerateNewCustomRoster((result) => { Debug.Log("Post New Roster: "+result); });
        //dbController.PostNewRoster((result) => { Debug.Log(result); });
        if (GUILayout.Button("Post New Clean Month"))
            dbController.PostNewCleanMonth((result) => { Debug.Log("Post New Clean Month: "+result); });
        GUILayout.Space(20f);

        if (GUILayout.Button("Restart DB"))
            dbController.ResetDB();
        if (GUILayout.Button("Clean DB"))
            dbController.GetCleanDatabase((result) => { Debug.Log("Clean DB: "+result); });
        GUILayout.Space(20f);

        if (GUILayout.Button("GetTest"))
            dbController.GetTest((result) => { Debug.Log("Get Test: "+result); });
        if (GUILayout.Button("PostTest"))
            dbController.PostTest((result) => { Debug.Log("Post Test: "+result); });
        GUILayout.Space(20f);
        #endregion
    }

}
