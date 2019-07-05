using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Test : MonoBehaviour
{
    public TestCalendar calendar = null;
    public TestCalendar auxCalendar = null;


    private void Start()
    {
        calendar.CustomLoadRaiders();
        auxCalendar = calendar;

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            calendar.raiders.RemoveAt(0);

    }
}
