using UnityEngine;

public class HoveringSpike : ObstacleBase
{
    [Header("Hover Settings")]
    [SerializeField] private Vector3 localOffset = new Vector3(0, 2f, 0);
    [SerializeField] private float cycleTime = 2f;
    [SerializeField] private AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float timer;

    protected override void Awake()
    {
        base.Awake();

        startPosition = transform.position;
        targetPosition = startPosition + localOffset;
    }

    protected override void ActiveUpdate()
    {
        HandleHover();
    }

    private void HandleHover()
    {
        timer += Time.deltaTime;

        if (timer >= cycleTime)
            timer -= cycleTime;

        float normalizedTime = timer / cycleTime;

        float t;

        if (normalizedTime <= 0.5f)
        {
            // Going up
            float halfT = normalizedTime / 0.5f;
            t = motionCurve.Evaluate(halfT);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        else
        {
            // Coming down
            float halfT = (normalizedTime - 0.5f) / 0.5f;
            t = motionCurve.Evaluate(halfT);
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
        }
    }
}
