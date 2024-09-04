using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform Player;
    public ObstacleData ObstacleData;

    private int gridWidth = 10;
    private int gridHeight = 10;
    private float tileSize = 1.3f; 

    private Vector3 targetPosition;
    private bool isMoving = false;
    private List<Vector3> path = new List<Vector3>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CubeManager cube = hit.collider.GetComponent<CubeManager>();
                if (cube != null)
                {
                    targetPosition = hit.collider.transform.position;
                    FindGrid(Player.position, targetPosition);
                }
            }
        }

        if (path.Count > 0 && !isMoving)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    private void FindGrid(Vector3 start, Vector3 end)
    {
        Vector2Int startGridPos = WorldToGridPosition(start);
        Vector2Int endGridPos = WorldToGridPosition(end);

        // A* Algorithm
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        Node startNode = new Node(startGridPos, null, 0, GetMovementDistance(startGridPos, endGridPos));
        Node endNode = new Node(endGridPos, null, 0, 0);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Position == endNode.Position)
            {
                ReversePath(startNode, currentNode);
                return;
            }

            foreach (Vector2Int neighbourPos in GetNeighbours(currentNode.Position))
            {
                if (ObstacleData.IsObstacle(neighbourPos.x, neighbourPos.y) || closedList.Contains(new Node(neighbourPos)))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.GCost + GetMovementDistance(currentNode.Position, neighbourPos);
                Node neighbourNode = new Node(neighbourPos, currentNode, newMovementCostToNeighbour, GetMovementDistance(neighbourPos, endGridPos));

                if (newMovementCostToNeighbour < neighbourNode.GCost || !openList.Contains(neighbourNode))
                {
                    neighbourNode.GCost = newMovementCostToNeighbour;
                    neighbourNode.HCost = GetMovementDistance(neighbourPos, endGridPos);
                    neighbourNode.Parent = currentNode;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
    }

    private void ReversePath(Node startNode, Node endNode)
    {
        List<Vector3> newPath = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(new Vector3(currentNode.Position.x * tileSize, 0f, currentNode.Position.y * tileSize));
            currentNode = currentNode.Parent;
        }

        newPath.Reverse();
        path = newPath;
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int z = Mathf.RoundToInt(worldPosition.z / tileSize);
        return new Vector2Int(x, z);
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * tileSize, 0f, gridPosition.y * tileSize);
    }

    private int GetMovementDistance(Vector2Int a, Vector2Int b)
    {
        int dstX = Mathf.Abs(a.x - b.x);
        int dstZ = Mathf.Abs(a.y - b.y);
        return dstX + dstZ;
    }

    private IEnumerable<Vector2Int> GetNeighbours(Vector2Int gridPos)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (gridPos.x - 1 >= 0) neighbours.Add(new Vector2Int(gridPos.x - 1, gridPos.y));
        if (gridPos.x + 1 < gridWidth) neighbours.Add(new Vector2Int(gridPos.x + 1, gridPos.y));
        if (gridPos.y - 1 >= 0) neighbours.Add(new Vector2Int(gridPos.x, gridPos.y - 1));
        if (gridPos.y + 1 < gridHeight) neighbours.Add(new Vector2Int(gridPos.x, gridPos.y + 1));

        return neighbours;
    }

    private IEnumerator MoveAlongPath()
    {
        isMoving = true;

        foreach (Vector3 point in path)
        {
            while (Vector3.Distance(Player.position, point) > 0.001f)
            {
                Player.position = Vector3.MoveTowards(Player.position, point, Time.deltaTime * 5f);
                yield return null;
            }
        }
        path.Clear();

        isMoving = false;
    }

    private class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public int GCost; 
        public int HCost; 

        public int FCost { get { return GCost + HCost; } }

        public Node(Vector2Int position, Node parent = null, int gCost = 0, int hCost = 0)
        {
            Position = position;
            Parent = parent;
            GCost = gCost;
            HCost = hCost;
        }
    }
}
