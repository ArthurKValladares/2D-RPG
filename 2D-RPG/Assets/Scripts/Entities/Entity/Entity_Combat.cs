using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private int damage;

    public void PerformAttack()
    {
        foreach (Collider2D targets in GetDetectedColliders())
        {
            Entity_Health healthComponent = targets.GetComponent<Entity_Health>();
            if (healthComponent)
            {
                healthComponent.TakeDamage(damage, transform);
            }
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
