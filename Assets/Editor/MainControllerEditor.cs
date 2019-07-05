using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainController))]
public class MainControllerEditor : Editor
{
    [Header("Debug Purpouse")]
    [Space()]
    [SerializeField] private int raiderID = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        MainController mainController = (MainController)target;
        raiderID = EditorGUILayout.IntField("Raider ID:",raiderID);
        if (GUILayout.Button("Change Current User"))
        {
            mainController.ChangeCurrentRaider(raiderID);
        }
    }
}
