using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField] public Vector3 location;
    [SerializeField] public bool walkable = true;
    [SerializeField] public Node previousNode;
    [SerializeField] public int gCost;
    [SerializeField] public int fCost;
    [SerializeField] public int hCost;


    public Node(Vector3 newLocation)
    {
        location = newLocation;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
