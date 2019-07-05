using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WebRequest;

public class WebController : Singleton<WebController>
{
    [SerializeField] public List<WebRequest> webRequests = new List<WebRequest>();

    public void Start()
    {
        //CallWebRequest(GetDate());
        //GetWebRequest("https://jsonplaceholder.typicode.com/todos/1", () => { print("Done!"); });        
        //PostWebRequest("http://localhost/nameFolerorwhatever/PostExample.php", () => { print("Done Form!"); }, new FormField("loginUser","myData"));        
    }
    public void GetWebRequest(string url = "https://jsonplaceholder.typicode.com/todos/1", Action<string> OnCompleteAction = null)
    {
        WebRequest webRequest = new WebRequest(url, OnCompleteAction);
        webRequests.Add(webRequest);
        webRequest.Get();
    }
    public void PostWebRequest(string url = "https://jsonplaceholder.typicode.com/todos/1", Action<string> OnCompleteAction = null, params FormField[] forms)
    {
        WebRequest webRequest = new WebRequest(url, OnCompleteAction);
        webRequests.Add(webRequest);
        webRequest.Post(forms);
    }
}
