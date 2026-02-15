using Photon.Pun;
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
        double elapsed = PhotonNetwork.Time - networkStartTime;

        float angle = (float)(elapsed * rotationSpeed);

        Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis.normalized);

        if (useLocalRotation)
        {
            target.localRotation = rotation;
        }
        else
        {
            target.rotation = rotation;
        }
    }


}
