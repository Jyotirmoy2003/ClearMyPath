using UnityEngine;

public class SimpleRotator : ObstacleBase
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up; // Y axis by default
    public float rotationSpeed = 60f;         // Degrees per second
    public bool useLocalRotation = true;
    [SerializeField] Transform target;

    

    protected override void ActiveUpdate()
    {
        Vector3 rotation = rotationAxis.normalized * rotationSpeed * Time.deltaTime;

        if (useLocalRotation)
        {
            target.Rotate(rotation, Space.Self);
        }
        else
        {
            target.Rotate(rotation, Space.World);
        }
    }

}
