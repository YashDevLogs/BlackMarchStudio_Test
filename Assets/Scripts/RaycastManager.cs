using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private TextMeshProUGUI InfoTextUI;

        // Update is called once per frame
        void Update()
        {
            GetCubeInfo();
        }

        private void GetCubeInfo()
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                CubeManager cubeManager = hit.collider.gameObject.GetComponent<CubeManager>();
                if(cubeManager != null)
                {
                    ShowCubePos(cubeManager);
                }
            }
        }

        private void ShowCubePos(CubeManager cubeManager)
        {
            InfoTextUI.text = $"Cube Position on X & Z : ({cubeManager.PositionX}, {cubeManager.PositionY}) ";
        }
    }
}