using UnityEngine;

public class ObstacleService : MonoBehaviour
{
    public ObstacleData obstacleData; 
    public GameObject obstaclePrefab;  

    // Start is called before the first frame update
    void Start()
    {
        CreateObstacles();
    }

    public void CreateObstacles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate obstacles based on the obstacleData
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if (obstacleData.IsObstacle(x, z))
                {
                    GameObject cube = GameObject.Find($"Cube_{x}_{z}");
                    if (cube != null)
                    {
                        Vector3 position = cube.transform.position + Vector3.up * 0.5f;
                        Instantiate(obstaclePrefab, position, Quaternion.identity, transform);
                        Debug.Log($"Obstacle placed at ({x}, {z})");
                    }
                }
            }
        }
    }
}
