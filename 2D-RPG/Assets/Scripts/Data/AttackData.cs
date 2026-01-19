using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;

public class AttackData
{
    private Entity_Stats stats;
    private DamageScaleData damageScaleData;
    private ElementalDamageType primaryElementalDamage;
    private ElementalDamageType secondaryElementalDamage;

    public PhysicalDamageInfo physicalDamageInfo;
    public ElementalDamageInfo elementalDamageInfo;

    public AttackData(Entity_Stats stats, DamageScaleData damageScaleData, ElementalDamageType primaryElementalDamage, ElementalDamageType secondaryElementalDamage)
    {
        this.stats = stats;
        this.damageScaleData = damageScaleData;
        this.primaryElementalDamage = primaryElementalDamage;
        this.secondaryElementalDamage = secondaryElementalDamage;

        this.physicalDamageInfo = stats.CalculatePhysicalDamage(damageScaleData.phyiscal);
        this.elementalDamageInfo = stats.CalculateElementalDamage(primaryElementalDamage, secondaryElementalDamage, damageScaleData.secondaryElementMultiplier, damageScaleData.elemental);
    }

    public void ApplyElementalEffect(Entity_VFX entityVFX, Collider2D target, HitInfo hitInfo)
    {
        if (hitInfo.didHit)
        {
            entityVFX.CreateOnHitTargetVFX(target.transform, physicalDamageInfo.wasCritical, primaryElementalDamage);

            if (!hitInfo.killedVictim && primaryElementalDamage != ElementalDamageType.None)
            {
                Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
                if (statusHandler)
                {
                    ElementalEffectData effectData = new ElementalEffectData(stats, damageScaleData);
                    statusHandler.ApplyStatusEffect(primaryElementalDamage, effectData);
                }
            }
        }
    }
}
