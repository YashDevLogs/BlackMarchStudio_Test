using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public int PositionX;
    public int PositionY;

    public void SetPosition(int x, int y)
    {
        PositionX = x;
        PositionY = y;
    }
}
