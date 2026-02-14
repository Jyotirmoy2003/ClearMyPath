using UnityEngine;

public class RagdollSwitcher : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private Collider mainCollider;

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (mainRigidbody == null)
            mainRigidbody = GetComponent<Rigidbody>();

        if (mainCollider == null)
            mainCollider = GetComponent<Collider>();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    [NaughtyAttributes.Button]
    public void EnableRagdoll()
    {
        animator.enabled = false;

        mainRigidbody.isKinematic = true;
        mainCollider.enabled = false;

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in ragdollColliders)
        {
            // Skip main collider
            if (col == mainCollider)
                continue;

            col.enabled = false;
        }

        if (mainRigidbody != null)
            mainRigidbody.isKinematic = false;

        if (mainCollider != null)
            mainCollider.enabled = true;

        if (animator != null)
            animator.enabled = true;
    }
}
