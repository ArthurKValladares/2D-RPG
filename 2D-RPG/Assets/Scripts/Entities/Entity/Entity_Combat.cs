using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;
    public ElementalDamageType primaryElementalDamage = ElementalDamageType.None;
    public ElementalDamageType secondaryElementalDamage = ElementalDamageType.None;

    [Header("Target Detection")]
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private Transform targetCheck;
    [SerializeField] private LayerMask whatIsTarget;

    [Header("Status Effect Duration")]
    [SerializeField] private float defaultStatusDuration = 3.0f;
    [SerializeField] private float chillSlowPercentage = 0.5f;
    [SerializeField] private int burnTicksPerSecond = 2;
    [SerializeField] private float electrifyChargePerApplication = 0.2f;

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
            if (damagable == null) continue;

            PhysicalDamageInfo physicalDamageInfo = stats.CalculatePhysicalDamage(basicAttackScale.phyiscal);
            ElementalDamageInfo elementalInfo = stats.CalculateElementalDamage(primaryElementalDamage, secondaryElementalDamage, basicAttackScale.secondaryElementMultiplier, basicAttackScale.elemental);

            HitInfo hitInfo = damagable.TakeDamage(physicalDamageInfo, elementalInfo, transform);
            if (hitInfo.didHit)
            {
                entityVFX.CreateOnHitTargetVFX(target.transform, physicalDamageInfo.wasCritical, elementalInfo.primaryType);

                if (!hitInfo.killedVictim && primaryElementalDamage != ElementalDamageType.None)
                {
                    ElementalEffectData effectData = new ElementalEffectData(stats, basicAttackScale);

                    Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
                    statusHandler.ApplyStatusEffect(primaryElementalDamage, effectData);
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
