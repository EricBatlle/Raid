using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonClass
{
    [SerializeField] public int userId = 0;
    [SerializeField] private int id = 0;
    [SerializeField] private string title = "delectus aut autem";
    [SerializeField] private bool completed = false;    
}
