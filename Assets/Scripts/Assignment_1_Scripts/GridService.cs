using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridService : MonoBehaviour
{
    public GameObject CubePrefab;
    public int GridWidth = 10;
    public int GridHeight = 10;


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for(int x = 0; x <= GridWidth; x++)
        {
            for(int z =0; z <= GridHeight; z++)
            {
                Vector3 position = new Vector3(x * 1.3f ,0,z * 1.3f);
                GameObject cube = Instantiate(CubePrefab, position, Quaternion.identity);
                cube.name = $"Cube_{x}_{z}"; 

                CubeManager cubeManager = cube.GetComponent<CubeManager>();
                if(cubeManager != null)
                {
                    cubeManager.SetPosition(x, z);
                }
            }
        }
    }


}
