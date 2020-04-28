using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using System.IO;
using UnityEngine.UI;
using Firebase.Unity.Editor;
using Firebase.Database;
using SimpleJSON;

public class DataDownloadAndSave : MonoBehaviour { 

    // To display message about the internet connectivity
    public GameObject msgBox;
    string jsonData;
    private string DataPath;
    JSONNode jsonNode;
    public bool InternetActive = false;
    public List<string> mapnames;
    public Dropdown mapslist;
    public static string initialmapname;
    public NavController nav;

    void Start()
    {
        StartCoroutine(checkInternetConnection());
    }
    
    IEnumerator checkInternetConnection()
    {
        yield return new WaitForSeconds(5);
        WWW www = new WWW("https://indoardatabase.firebaseio.com/");
        yield return www;
        if (www.error != null)
        {
            InternetActive = false;
            Debug.Log("Internet is not working fine");
            msgBox.SetActive(true);
        }
        else
        {
            InternetActive = true;
            Debug.Log("Internet is working perfectly fine");
            DataPath = Path.Combine(Application.streamingAssetsPath, "MapData.txt");
            StartCoroutine(DataFetcher());
        }
    }

    public void OnClickRetry()
    {
        StartCoroutine(checkInternetConnection());
        msgBox.SetActive(false);
    }

    IEnumerator DataFetcher()
    {
        string url = "https://indoardatabase.firebaseio.com/.json";
        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            jsonData = www.text;
        }
        jsonNode = SimpleJSON.JSON.Parse(jsonData);

        using (StreamWriter streamWriter = File.CreateText(DataPath))
        {
            streamWriter.Write(jsonData);
        }
        Debug.Log("File Created");

        for (int i = 0; i < jsonNode.Count; i++)
        {

            if (String.IsNullOrEmpty(jsonNode[i]["name"]))
            {
                // To escape from other nodes that do not have name attribute
            }
            else
            {
                mapnames.Add(jsonNode[i]["name"]);
            }
        }

        mapslist.AddOptions(mapnames);
    }

    public void selectmap(int index)
    {
        if (index == 0)
        {
        }
        else
        {
            initialmapname = mapnames[index];
            nav.mapname = initialmapname;
            Debug.Log(nav.mapname);
            SceneManager.LoadScene(1);// Inactive the first panel to make the camera and second UI visible
        }
    }
}
