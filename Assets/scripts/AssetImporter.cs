using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System.IO;
using System;
using UnityEngine.UI;
using Firebase.Unity.Editor;
using Firebase.Database;
using SimpleJSON;


public class AssetImporter : MonoBehaviour
{
    private string DataPath;
    public Dropdown destinationlist;
    string jsonData;
    string destname;
    public DataDownloadAndSave dataSave;
    public Material invismat;
    public List<String> destinationNames;
    public List<String> mapnames;
    public List<GameObject> prelist;
    public GameObject WayPointPrefab;
    public GameObject DestinationPrefab;
    public GameObject TextPrefab;
    public NavController nav;
    public List<GameObject> nodeList;
    private Node[] Map = new Node[0];
    JSONNode jsonNode;
    string jsonString;
    public List<String> List_of_elements;

    void Start()
    {
        if (dataSave.InternetActive)
        {
            DataPath = Path.Combine(Application.streamingAssetsPath, "MapData.txt");
            destinationNames.Add("destination");
            using (StreamReader streamReader = File.OpenText(DataPath))
            {
                jsonString = streamReader.ReadToEnd();
                jsonNode = SimpleJSON.JSON.Parse(jsonData);
            }
            Debug.Log("Data Loaded");
            Debug.Log(jsonString);
        }
    }

    public void test()
    {
        foreach (KeyValuePair<string, JSONNode> kvp in jsonNode[SceneLoader.initialmapname])
        {
            if (GameObject.Find(kvp.Key.ToString()) == null)
            {
                if (String.Compare(kvp.Key[0].ToString(), "W") == 0)
                {
                    CreatePrefab(WayPointPrefab, new Vector3(jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posX"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posY"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posZ"]), new Quaternion(jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotX"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotY"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotZ"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotW"]), kvp.Key.ToString());
                    //Debug.Log("In the test");

                }
                else if (String.Compare(kvp.Key[0].ToString(), "D") == 0)
                {
                    CreatePrefab(DestinationPrefab, new Vector3(jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posX"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posY"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["posZ"]), new Quaternion(jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotX"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotY"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotZ"], jsonNode[SceneLoader.initialmapname][kvp.Key.ToString()]["rotW"]), kvp.Key.ToString());

                }
            }
        }
        if (destinatioNames.Count == 1)
        {
            findDestination();
        }
    }

    public void findDestination()
    {
        Debug.Log("under test");
        Debug.Log(SceneLoader.initialmapname);
        Debug.Log("scene loaded");
        if (SceneLoader.initialmapname != "")
        {
            foreach (KeyValuePair<string, JSONNode> kvp in jsonNode[SceneLoader.initialmapname])
            {
                if (String.Compare(kvp.Key[0].ToString(), "D") == 0)
                {
                    destinatioNames.Add(jsonNode[SceneLoader.initialmapname][kvp.Key]["name"]);

                }
            }
        }
        destinationlist.AddOptions(destinatioNames);
    }

    public void CallNavigation(int index)
    {


        string destname = destinatioNames[index];
        Debug.Log(SceneLoader.initialmapname);
        nav.StartNavigation(destname);
    }
    public void CreatePrefab(GameObject name, Vector3 pos, Quaternion rot, String namepre)
    {

        var temp = Instantiate(name, pos, rot);
        temp.name = namepre;
        prelist.Add(temp);
        temp.gameObject.GetComponent<MeshRenderer>().enabled = false;


        nodeList.Add(temp);

        Map = new Node[nodeList.Count];
        int i = 0;
        foreach (var node in nodeList)
        {
            Map[i] = node.GetComponent<Node>();
            i = i + 1;
        }
        nav.allnodes = Map;
    }


    public void UpdatePrefab()
    {

        StartCoroutine(DataFetcher());
        foreach (var child in prelist)
        {
            //Debug.Log(child.name);
            GameObject temp = GameObject.Find(child.name);
            temp.transform.position = new Vector3(jsonNode[SceneLoader.initialmapname][child.name]["posX"], jsonNode[SceneLoader.initialmapname][child.name]["posY"], jsonNode[SceneLoader.initialmapname][child.name]["posZ"]);
            temp.transform.rotation = new Quaternion(jsonNode[SceneLoader.initialmapname][child.name]["rotX"], jsonNode[SceneLoader.initialmapname][child.name]["rotY"], jsonNode[SceneLoader.initialmapname][child.name]["rotZ"], jsonNode[SceneLoader.initialmapname][child.name]["rotW"]);

        }
    }

    public void ClearPath()
    {
        foreach (var child in prelist)
        {
            Destroy(GameObject.Find(child.name));
        }
        prelist.Clear();
        CancelInvoke();
    }
}