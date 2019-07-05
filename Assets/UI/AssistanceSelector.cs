using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssistanceSelector : MonoBehaviour
{
    public enum AssistanceType { Assist, Late, Decline, None };
    [SerializeField] private Button assistButton = null;
    [SerializeField] private Button lateButton = null;
    [SerializeField] private Button declineButton = null;
    [Space()]
    [SerializeField] private Dropdown dropdown = null;

    private void Awake()
    {
        assistButton.onClick.AddListener(() => { OnAssistanceTypeClick(AssistanceType.Assist); });
        lateButton.onClick.AddListener(() => { OnAssistanceTypeClick(AssistanceType.Late); });
        declineButton.onClick.AddListener(() => { OnAssistanceTypeClick(AssistanceType.Decline); });
    }

    private void OnAssistanceTypeClick(AssistanceType assistanceType)
    {
        //If Late, check how late
        if (assistanceType == AssistanceType.Late)
        {
            UIController.s_Instance.OnAssistanceTypeClick(assistanceType,GetHowManyLateMinutes());
        }
        else
        {
            UIController.s_Instance.OnAssistanceTypeClick(assistanceType);
        }
    }

    private int GetHowManyLateMinutes()
    {
        //0 -> 15
        //1 -> 30
        //...
        return 15 + (dropdown.value * 15);
    }
    
}
