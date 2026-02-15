using Photon.Pun;
using UnityEngine;

public class HammerObstacle : ObstacleBase
{
    [SerializeField] LayerMask platfromLayer;
    [SerializeField] AudioSource slamAudio;
    [Header("Rotation Settings")]
    [SerializeField] private float startAngle = 0f;
    [SerializeField] private float hitAngle = -90f;
    [SerializeField] private float totalCycleTime = 2f;

    [Header("Motion Curve")]
    [SerializeField] private AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] ParticleSystem mashEffectParticle;

    

    private float timer = 0f;
   

    private void Start()
    {
        // Initialize rotation at start angle
        SetRotation(startAngle);
    }

    

    protected override void ActiveUpdate()
    {
        HandleSwing();
    }

    private void HandleSwing()
    {
        double elapsed = PhotonNetwork.Time - networkStartTime;

        float cycleTime = totalCycleTime;

        float normalizedTime = (float)(elapsed % cycleTime) / cycleTime;

        float angle;

        if (normalizedTime <= 0.5f)
        {
            float t = normalizedTime / 0.5f;
            float curved = motionCurve.Evaluate(t);
            angle = Mathf.Lerp(startAngle, hitAngle, curved);
        }
        else
        {
            float t = (normalizedTime - 0.5f) / 0.5f;
            float curved = motionCurve.Evaluate(t);
            angle = Mathf.Lerp(startAngle,hitAngle,curved);
        }

        SetRotation(angle);
    }


    private void SetRotation(float angle)
    {
        Vector3 rot = transform.localEulerAngles;
        rot.z = angle;
        transform.localEulerAngles = rot;
    }



    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Debug.Log("trigger layer tag:" +other.gameObject.layer +" and our layer "+platfromLayer.value);
        if ((platfromLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            mashEffectParticle.Play();
            slamAudio.Stop();
            slamAudio.Play();
        }
    }

   

    
}
