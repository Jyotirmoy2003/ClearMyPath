using UnityEngine;

public class HammerObstacle : ObstacleBase
{
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
        timer += Time.deltaTime;

        if (timer >= totalCycleTime)
            timer -= totalCycleTime;

        float normalizedTime = timer / totalCycleTime;

        float angle;

        if (normalizedTime <= 0.5f)
        {
            // Forward swing
            float t = normalizedTime / 0.5f; // Remap 0–0.5 to 0–1
            float curved = motionCurve.Evaluate(t);
            angle = Mathf.Lerp(startAngle, hitAngle, curved);
        }
        else
        {
            // Return swing
            float t = (normalizedTime - 0.5f) / 0.5f; // Remap 0.5–1 to 0–1
            float curved = motionCurve.Evaluate(t);
            angle = Mathf.Lerp(startAngle, hitAngle, curved);
        }

        SetRotation(angle);
    }

    private void SetRotation(float angle)
    {
        Vector3 rot = transform.localEulerAngles;
        rot.z = angle;
        transform.localEulerAngles = rot;
    }

    

    

    
}
