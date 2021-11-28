using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PathfindingHandler))]
public class PathfindingInspector: Editor
{
    PathfindingHandler objectReff;

    public override void OnInspectorGUI()
    {
        objectReff = (PathfindingHandler)target;

        DrawDefaultInspector();
        if(GUILayout.Button("Generate"))
        {
            objectReff.nodes.GenerateNodes();
            Node[] tempX = Flatten<Node>(objectReff.nodes.nodes);
            Wrapper<Node> wrapper = new Wrapper<Node>();
            wrapper.Items = tempX;

            string jsonString = JsonUtility.ToJson(wrapper);
            System.IO.File.WriteAllText(Application.dataPath + "/PathfindingNodes.json", jsonString);

        }
    }


    public static T[] Flatten<T>(T[,] input)
    {
        int width = input.GetLength(0);
        int height = input.GetLength(1);
        T[] flattened = new T[width * height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                flattened[j * width + i] = input[i, j];

            }
        }

        return flattened;
    }


    public static T[,] Unflatten<T>(T[] input, int width, int height)
    {
        T[,] unflattened = new T[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                unflattened[i, j] = input[j * width + i];

            }
        }

        return unflattened;

    }
}

[System.Serializable]
public class Wrapper<T>
{
    public T[] Items;
}

[System.Serializable]
public class PathfindingHandler : MonoBehaviour
{
    [SerializeField]
    public Nodes nodes;

    int moveStraightCost = 10;
    int moveDiagonalCost = 24;


    float height = 0.1f;
    Vector3 nodeVector3;

    public static PathfindingHandler instance;

    void Awake()
    {
        instance = this;

        string savedNodes = System.IO.File.ReadAllText(Application.dataPath + "/PathfindingNodes.json");
        Node[] temp = JsonUtility.FromJson<Wrapper<Node>>(savedNodes).Items;
        nodes.nodes = PathfindingInspector.Unflatten<Node>(temp, nodes.amountOfNodes, nodes.amountOfNodes);
    }

    public void OnDrawGizmos()
    {
        nodeVector3 = new Vector3(nodes.nodeScale * 0.9f, transform.position.y + height, nodes.nodeScale * 0.9f);

        if (nodes.generated)
        {
            foreach (Node node in nodes.nodes)
            {

                if (node.walkable)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                Gizmos.DrawCube(node.location, nodeVector3);
            }
        }
    }


    Vector2 GetNodePosition(Vector3 worldPosition)
    {
        Vector2 nodeIndex = new Vector2();

        nodeIndex.x = Mathf.FloorToInt(worldPosition.x / nodes.nodeScale);

        nodeIndex.y = Mathf.FloorToInt(worldPosition.z / nodes.nodeScale);

        return nodeIndex;
    }

    public List<Node> GetPath(Vector3 startPoint,Vector3 endPoint)
    {
        Vector2 startNodePos = GetNodePosition(startPoint);
        Vector2 endNodePos = GetNodePosition(endPoint);

        Node startNode = nodes.nodes[(int)startNodePos.x, (int)startNodePos.y];
        Node endNode = nodes.nodes[(int)endNodePos.x, (int)endNodePos.y];

        List<Node> openList = new List<Node> { startNode };
        List<Node> closedList = new List<Node>();

        for (int i = 0; i < nodes.amountOfNodes; i++)
        {
            for(int j = 0; j < nodes.amountOfNodes; j++)
            {
                nodes.nodes[i, j].gCost = int.MaxValue;
                nodes.nodes[i, j].CalculateFCost();
                nodes.nodes[i, j].previousNode = null;

            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);

            if(currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            int tentativeGCost = 0;

            foreach (Node neighbourNode in GetNeighbourNodes(currentNode))
            {
                if (closedList.Contains(neighbourNode)) { continue; }

                tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);


                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    List<Node> GetNeighbourNodes(Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();

        int currentX = (int)currentNode.location.x;
        int currentY = (int)currentNode.location.z;

        if (currentX - 1 >= 0)
        {
            neighbourList.Add(nodes.nodes[currentX - 1, currentY]);

            if(currentY - 1 >= 0)
            {
                neighbourList.Add(nodes.nodes[currentX - 1, currentY - 1]);
            }
            if(currentY + 1 < nodes.amountOfNodes)
            {
                neighbourList.Add(nodes.nodes[currentX - 1, currentY + 1]);
            }
        }

        if(currentX + 1 < nodes.amountOfNodes)
        {
            neighbourList.Add(nodes.nodes[currentX + 1, currentY]);

            if (currentY - 1 >= 0)
            {
                neighbourList.Add(nodes.nodes[currentX + 1, currentY - 1]);
            }
            if (currentY + 1 < nodes.amountOfNodes)
            {
                neighbourList.Add(nodes.nodes[currentX + 1, currentY + 1]);
            }
        }

        if (currentY - 1 >= 0)
        {
            neighbourList.Add(nodes.nodes[currentX, currentY - 1]);
        }

        if (currentY + 1 < nodes.amountOfNodes)
        {
            neighbourList.Add(nodes.nodes[currentX, currentY + 1]);
        }

        return neighbourList;
    }

    List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();

        path.Add(endNode);

        Node currentNode = endNode;

        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    int CalculateDistanceCost(Node nodeA, Node nodeB)
    {
        int xDistance = (int)Mathf.Abs(nodeA.location.x - nodeB.location.x);
        int zDistance = (int)Mathf.Abs(nodeA.location.z - nodeB.location.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return moveDiagonalCost * Mathf.Min(xDistance, zDistance) + moveStraightCost * remaining;
    }


    Node GetLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];

        for(int i = 1; i < nodeList.Count; i++)
        {
            if(nodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }

        return lowestFCostNode;
    }
}
