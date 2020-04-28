using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class NavController : MonoBehaviour
{
    public Node[] allnodes;
    private Node target;
    public List<Node> path = new List<Node>();
    private string jsonData;
    JSONNode jsonNode;

    public Material waymat;
    public Material invi;
    public Material black;
    public Material side;


    public AssetImporter assetImporter;
    public string mapname;
    public float thresold;

    void Start()
    {
        StartCoroutine(DataFetcher());
        Thresold();
    }

    public void Thresold()
    {
        //thresold = float.Parse(assetImporter.destinatoion.text);
        thresold = 2f;
    }

    IEnumerator DataFetcher()
    {
        string url = "https://cloudtest-179e5.firebaseio.com/.json";
        WWW www = new WWW(url);
        yield return www;

        // store text in www to json string
        if (string.IsNullOrEmpty(www.error))
        {
            jsonData = www.text;
        }

        // use simpleJSON to get values stored in JSON data for different key value pair
        jsonNode = SimpleJSON.JSON.Parse(jsonData);
    }

    Node ReturnClosestNode(Node[] nodes, Vector3 point)
    {
        float minDist = Mathf.Infinity;
        Node closestNode = null;
        foreach (Node node in nodes)
        {
            float dist = Vector3.Distance(node.pos, point);
            if (dist < minDist)
            {
                closestNode = node;
                minDist = dist;
            }
        }
        return closestNode;
    }



    public void StartNavigation(string DestName)
    {
        if (path != null)
        {
            foreach (Node obj in path)
            {
                obj.GetComponent<MeshRenderer>().enabled = false;
                //Debug.Log(obj.gameObject.name);
            }
        }

        foreach (KeyValuePair<string, JSONNode> kvp in jsonNode[SceneLoader.initialmapname]) //destname needs updation
        {

            if (string.Compare(kvp.Key[0].ToString(), "D") == 0)
            {
                if (DestName == jsonNode[SceneLoader.initialmapname][kvp.Key]["name"]) // second destname needs updation
                {
                    target = GameObject.Find(kvp.Key).GetComponent<Node>();//destname needs updation
                }
            }
        }
        Node closestNode = ReturnClosestNode(allnodes, Camera.main.transform.position);
        //Debug.Log(closestNode.gameObject.name);
        foreach (Node node in allnodes)
        {
            node.FindNeighbors(thresold);
        }

        //get path from A* algorithm
        //Debug.Log("PATHCOUNT: " + target);
        path = this.gameObject.GetComponent<AStar>().FindPath(closestNode, target, allnodes);

        foreach (Node obj in path)
        {
            obj.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
