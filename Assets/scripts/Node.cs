using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public Vector3 pos;

    [Header("A*")]
    public List<Node> neighbors = new List<Node>();
    public float FCost { get { return GCost + HCost; } }
    public float HCost { get; set; }
    public float GCost { get; set; }
    public float Cost { get; set; }
    public Node Parent { get; set; }

    //next node in navigation list
    public Node NextInList { get; set; }

    private void Start()
    {
        //transform.GetChild (0).gameObject.SetActive (false);

        pos = this.gameObject.transform.position;
        Debug.Log(pos);
    }

    public void Activate(bool active)
    {
        //transform.GetChild (0).gameObject.SetActive (active);
        if (NextInList != null)
        {
            transform.LookAt(NextInList.transform);
        }
    }

    public void FindNeighbors(float Thresold)
    {
        Debug.Log(Thresold);
        foreach (Node node in FindObjectsOfType<Node>())
        {
            if (Vector3.Distance(node.pos, pos) < Thresold)
            {
                neighbors.Add(node);
            }
        }
    }
}
