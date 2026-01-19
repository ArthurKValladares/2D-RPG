using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [Header("Target Checking")]
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float targetCheckRadius = 1.0f;
    [Header("Surround Checking")]
    [SerializeField] protected float surroundCheckRadius = 10.0f;

    protected Entity_Stats playerStats;

    protected DamageScaleData damageScaleData;
    protected ElementalDamageType primaryElementalDamage;
    protected ElementalDamageType secondaryElementalDamage;

    private void Awake()
    {
        if (targetCheck == null)
        {
            targetCheck = transform;
        }
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        Collider2D[] enemies = EnemiesAround(t, radius);
        foreach (Collider2D target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            //
            // TODO: This is very copy-paste from Entity_Combat
            //
            PhysicalDamageInfo physicalDamageInfo = playerStats.CalculatePhysicalDamage(damageScaleData.phyiscal);
            ElementalDamageInfo elementalInfo = playerStats.CalculateElementalDamage(primaryElementalDamage, secondaryElementalDamage, damageScaleData.secondaryElementMultiplier, damageScaleData.elemental);

            HitInfo hitInfo = damagable.TakeDamage(physicalDamageInfo, elementalInfo, transform);

            if (hitInfo.didHit)
            {
                // TODO: on hit VFX?
                //entityVFX.CreateOnHitTargetVFX(target.transform, physicalDamageInfo.wasCritical, elementalInfo.primaryType);

                if (!hitInfo.killedVictim && primaryElementalDamage != ElementalDamageType.None)
                {
                    Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
                    if (statusHandler)
                    {
                        ElementalEffectData effectData = new ElementalEffectData(playerStats, damageScaleData);
                        statusHandler.ApplyStatusEffect(primaryElementalDamage, effectData);
                    }
                }
            }
            //
            //
            //
        }
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in EnemiesAround(transform, surroundCheckRadius))
        {
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetCheck.position, surroundCheckRadius);
    }
}
