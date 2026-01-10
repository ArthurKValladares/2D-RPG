using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stats;

    [Header("Target Detection")]
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status Effect Duration")]
    [SerializeField] private float defaultStatusDuration = 3.0f;
    [SerializeField] private float chillSlowPercentage = 0.5f;
    [SerializeField] private float burnDamagePerTick = 1.0f;
    [SerializeField] private float electrifyChargePerApplication = 0.2f;
    [SerializeField] private float electrifyDamage = 10.0f;

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
                ElementalDamageInfo elementalInfo = stats.CalculateElementalDamage(ElementalDamageType.Ice);

                bool tookDamage = damagable.TakeDamage(physicalDamageInfo.damageResult, elementalInfo, transform);
                if (tookDamage)
                {
                    entityVFX.CreateOnHitTargetVFX(target.transform, physicalDamageInfo.wasCritical, elementalInfo.primaryType);
                    ApplyStatusEffect(target, elementalInfo.primaryType);
                }
            }
        }
    }

    private void ApplyStatusEffect(Collider2D target, ElementalDamageType ty)
    {
        if (ty == ElementalDamageType.None) return;

        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if (!statusHandler) return;

        if (statusHandler.CanApply(ty))
        {
            switch (ty)
            {
                case ElementalDamageType.Ice:
                    statusHandler.ApplyChillEffect(defaultStatusDuration, chillSlowPercentage);
                    break;
                case ElementalDamageType.Fire:
                    statusHandler.ApplyBurnEffect(defaultStatusDuration, burnDamagePerTick);
                    break;
                case ElementalDamageType.Lightning:
                    statusHandler.ApplyElectrifyEffect(electrifyChargePerApplication, electrifyDamage);
                    break;
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
