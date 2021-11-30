using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nodes", menuName = "Pathfinding/Nodes", order = 1)]
[System.Serializable]
public class Nodes : ScriptableObject
{
    public LayerMask blockingMask;
    public float nodeScale = 1;

    [SerializeField]
    public Node[,] nodes;
    public bool generated = false;

    [SerializeField]
    [HideInInspector]
    public int amountOfNodes;

    public void GenerateNodes()
    {
        amountOfNodes = 200;

        nodes = new Node[amountOfNodes, amountOfNodes];
        RaycastHit[] hits = new RaycastHit[1];

        Ray tempRay = new Ray();

        for (int i = 0; i < amountOfNodes; i++)
        {
            for (int j = 0; j < amountOfNodes; j++)
            {
                Vector3 newLocation = new Vector3(i * nodeScale, 0, j * nodeScale);
                nodes[i, j] = new Node(newLocation);
                tempRay.origin = newLocation + Vector3.up * 30;
                tempRay.direction = -Vector3.up;
                nodes[i, j].walkable = true;

                if (Physics.SphereCastNonAlloc(tempRay,nodeScale/2, hits, 30,blockingMask) > 0)
                {
                    if (hits[0].transform.gameObject.isStatic)
                    {
                        nodes[i, j].walkable = false;
                    }
                }
            }
        }

        generated = true;

    }
}
