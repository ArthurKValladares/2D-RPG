using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stats;

    [Header("Target Detection")]
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach (Collider2D target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                DamageInfo physicalDamageInfo = stats.CalculatePhysicalDamage();
                // TODO: Need to get from damage source
                ElementalDamageInfo elementalInfo = stats.CalculateElementalDamage(ElementalDamageType.Fire, ElementalDamageType.Ice);

                bool tookDamage = damagable.TakeDamage(physicalDamageInfo.damageResult, elementalInfo, transform);
                if (tookDamage)
                {
                    entityVFX.CreateOnHitTargetVFX(target.transform, physicalDamageInfo.wasCritical);
                }
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
