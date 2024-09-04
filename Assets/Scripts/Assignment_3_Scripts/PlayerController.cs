
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Start()
    { 
        SetInitialPosition();
    }

    private void SetInitialPosition()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private void Update()
    {
        if (isMoving)
        {           
            MoveTowardsTargetGrid();
        }

    }

    public void MoveToGrid(Vector3 newPosition)
    {
        targetPosition = new Vector3(newPosition.x, 0f, newPosition.z);
        isMoving = true;
    }

    private void MoveTowardsTargetGrid()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            isMoving = false;
            transform.position = targetPosition; 

        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
