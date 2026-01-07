using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;

    [SerializeField] private int damage;

    [Header("Target Detection")]
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage, transform);
                entityVFX.CreateOnHitTargetVFX(target.transform);
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
