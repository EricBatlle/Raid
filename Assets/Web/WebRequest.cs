using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class WebRequest
{
    [Serializable]
    public class FormField
    {
        [SerializeField] public string fieldName = "fieldName";
        [SerializeField] public string value = "value";

        public FormField(string fieldName = "fieldName", string value = "value")
        {
            this.fieldName = fieldName;
            this.value = value;
        }
    }

    //Needs to be the same as the DBConst.php WebResponse class
    public static class WebResponse
    {
        private static string ERROR_HEADER = "ERROR: ";

        [SerializeField] public static string OK = "OK";
        [SerializeField] public static string ERROR = ERROR_HEADER + "Something went wrong :(";
        [SerializeField] public static string ERROR_DBCONNECTION = ERROR_HEADER + "Connection with the DB failed";
        [SerializeField] public static string ERROR_0RESULTS = ERROR_HEADER + "0 Results";
        [SerializeField] public static string ERROR_LOGIN_WRONG_CREDENTIALS = ERROR_HEADER + "Wrong Credentials";
        [SerializeField] public static string ERROR_LOGIN_UNEXISTANT_USERNAME = ERROR_HEADER + "Username does not exist";
        [SerializeField] public static string ERROR_REGISTER_DUPLICATE_USERNAME = ERROR_HEADER + "This username already exists";

        public static bool isResultOk(string result)
        {
            if(isEqualTo(WebResponse.OK,result))            
                return true;            
            else
            {
                if(!result.Contains(ERROR_HEADER))
                {
                    return true;
                }                
            }
            return false;
        }        
        public static bool isEqualTo(string webResponse, string result)
        {
            if (webResponse.Trim().Equals(result.Trim(), StringComparison.InvariantCultureIgnoreCase))
                return true;
            else
                return false;            
        }
    }

    [SerializeField] private Action<string> OnSuccesfullWebRequest = null;
    [SerializeField] private string url = "https://jsonplaceholder.typicode.com/todos/1";

    public WebRequest(string url = "https://jsonplaceholder.typicode.com/todos/1", Action<string> OnCompleteAction = null)
    {
        this.url = url;
        this.OnSuccesfullWebRequest = OnCompleteAction;
    }

    public void Get()
    {
        WebController.s_Instance.StartCoroutine(GetRequest());
    }
    public void Post(params FormField[] forms)
    {
        WebController.s_Instance.StartCoroutine(PostRequest(forms));
    }

    private IEnumerator GetRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
            {
                //Show results as text
                //print(www.downloadHandler.text);
                //Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;
                //Debug.Log("Get Request complete!");
                OnSuccesfullWebRequest(www.downloadHandler.text);
                RemoveFromWebControllerList();
            }
        }
    }
    private IEnumerator PostRequest(params FormField[] fields)
    {
        WWWForm form = new WWWForm();
        foreach(FormField fieldObject in fields)
        {
            form.AddField(fieldObject.fieldName, fieldObject.value);
        }
        using (UnityWebRequest www = UnityWebRequest.Post(url,form))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError)
                Debug.Log(www.error +"on "+url );
            else
            {
                //Debug.Log("Post Request complete!");
                OnSuccesfullWebRequest?.Invoke(www.downloadHandler.text);
                RemoveFromWebControllerList();
            }                
        }
    }
    
    private void RemoveFromWebControllerList()
    {
        WebController.s_Instance.webRequests.Remove(this);
    }
}