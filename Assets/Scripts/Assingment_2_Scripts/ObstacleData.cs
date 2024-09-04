using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ObstacleData", menuName = "Cube/ObstacleData", order = 1)]
public class ObstacleData : ScriptableObject
{
    public bool[,] obstacle = new bool[10, 10];
    public void PlaceObstacle(int x, int z, bool IsPlaced)
    {
        obstacle[x, z] = IsPlaced;
        Debug.Log("Obstacle placed on ObstacleData");
    }

    public bool IsObstacle(int x, int z)
    {
        return obstacle[x, z];
    }

}
