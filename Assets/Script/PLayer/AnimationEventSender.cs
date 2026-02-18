using UnityEngine;
using UnityEngine.Events;

public class AnimationEventSender : MonoBehaviour
{
    public UnityEvent<AnimationEvent> Event_OnFootstep;
    public UnityEvent<AnimationEvent> Event_OnLand;
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        Event_OnFootstep?.Invoke(animationEvent);
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        Event_OnLand?.Invoke(animationEvent);
    }
}
