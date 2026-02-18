using Photon.Pun;
using UnityEngine;

public class HoveringSpike : ObstacleBase
{
    [Header("Hover Settings")]
    [SerializeField] private Vector3 localOffset = new Vector3(0, 2f, 0);
    [SerializeField] private float cycleTime = 2f;
    [SerializeField] private AnimationCurve motionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPosition;
    private Vector3 targetPosition;

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
        double elapsed = PhotonNetwork.Time - networkStartTime;

        float normalizedTime = (float)(elapsed % cycleTime) / cycleTime;

        float t;

        if (normalizedTime <= 0.5f)
        {
            float halfT = normalizedTime / 0.5f;
            t = motionCurve.Evaluate(halfT);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
        else
        {
            float halfT = (normalizedTime - 0.5f) / 0.5f;
            t = motionCurve.Evaluate(halfT);
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
        }
    }

    [NaughtyAttributes.Button]
    void Init()
    {
        ListnToOnInitEvent(this,true);
    }

}
