using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Assignment_2_Scripts
{
    public class ObstacleEditor : EditorWindow
    {
        public ObstacleData Data;

        [MenuItem("Tools/Obstacle Editor")]
        public static void ShowWindow()
        {
            GetWindow<ObstacleEditor>("Obstacle Editor");
        }

        private void OnGUI()
        {
            Data = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", Data, typeof(ObstacleData), false);

            if (Data == null)
            {
                return;
            }
            Draw();
        }

        private void Draw()
        {
            for (int z = 0; z < 10; z++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < 10; x++)
                {
                    bool isBlocked = Data.IsObstacle(x, z);
                    bool newIsBlocked = GUILayout.Toggle(isBlocked, "", GUILayout.Width(20), GUILayout.Height(20));
                    if (newIsBlocked != isBlocked)
                    {
                        Data.PlaceObstacle(x, z, newIsBlocked);
                        EditorUtility.SetDirty(Data);
                        AssetDatabase.SaveAssets();

                        ObstacleService obstacleService = FindObjectOfType<ObstacleService>();
                        if (obstacleService != null)
                        {
                            obstacleService.CreateObstacles();
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
