using System.ComponentModel;
using UnityEngine;

public class DamageInfo
{
    public DamageInfo(float damageResult, bool wasCritical)
    {
        this.damageResult = damageResult;
        this.wasCritical = wasCritical;
    }

    public float damageResult;
    public bool wasCritical;
};

public class Entity_Stats : MonoBehaviour
{
    [Header("All Percentage values should be in the 0-100 range, not 0-1.")]

    [Header("Stats")]
    public Stat_MajorGroup majorStats;
    public Stat_OffensiveGroup offensiveStats;
    public Stat_DefensiveGroup deffensiveStats;

    [Header("Stats Multipliers")]
    public float vitalityHealthMultiplier = 5;
    public float vitalityArmorMultiplier = 1;
    public float agilityEvasionMultiplier = 0.5f;
    public float agilityCritChanceMultiplier = 0.3f;
    public float strengthDamageMultiplier = 1;
    public float strengthCritPowerMultiplier = 0.5f;
    public float intelligenceElementalDamageMultiplier = 1.0f;
    public float intelligenceElementalResistMultiplier = 0.5f;

    [Header("Stats Limits")]
    public float maxEvasion = 85.0f;
    public float armorMitigationConstant = 100;
    public float armorMitigationCap = 85.0f;
    public float secondaryElementMultiplier = 0.5f;
    public float elementalResistanceCap = 75.0f;

    public float CalculateMaxHP()
    {
        float baseHP = deffensiveStats.health.GetValue();
        float bonusHP = majorStats.vitality.GetValue() * vitalityHealthMultiplier;

        return baseHP + bonusHP;
    }

    public float CalculateEvasion()
    {
        float baseEvasion = deffensiveStats.evasion.GetValue();
        float bonusEvasion = majorStats.agility.GetValue() * agilityEvasionMultiplier;
        float clampedEvasion = Mathf.Clamp(baseEvasion + bonusEvasion, 0.0f, maxEvasion);
        
        return clampedEvasion / 100.0f;
    }

    private float GetArmor()
    {
        float baseArmor = deffensiveStats.armor.GetValue();
        float bonusArmor = majorStats.vitality.GetValue() * vitalityArmorMultiplier;

        return baseArmor + bonusArmor;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float armor = GetArmor();

        float clampedArmorReduction = Mathf.Clamp(armorReduction, 0.0f, 1.0f);
        float reductionMultiplier = 1.0f - clampedArmorReduction;
        float effectiveArmor = armor * reductionMultiplier;

        float armorMitigation = effectiveArmor / (effectiveArmor + armorMitigationConstant);

        return Mathf.Clamp(armorMitigation, 0, armorMitigationCap);
    }

    public float GetArmorReduction()
    {
        return offensiveStats.armorReduction.GetValue() / 100.0f;
    }

    private float GetCritPower()
    {
        float baseCritPower = offensiveStats.critPower.GetValue();
        float bonusCritPower = majorStats.strength.GetValue() * strengthCritPowerMultiplier;

        return (baseCritPower + bonusCritPower) / 100.0f;
    }

    private float GetCritChance()
    {
        float baseCritChance = offensiveStats.critChance.GetValue();
        float bonuesritChange = majorStats.agility.GetValue() * agilityCritChanceMultiplier;

        return (baseCritChance + bonuesritChange) / 100.0f;
    }

    public DamageInfo CalculatePhysicalDamage(float scaleFactor = 1.0f)
    {
        float baseDamage = offensiveStats.physicalDamage.GetValue();
        float bonusDamage = majorStats.strength.GetValue() * strengthDamageMultiplier;
        float totalBaseDamage = baseDamage + bonusDamage;

        bool wasCritical = Random.Range(0.0f, 1.0f) < GetCritChance();

        float damageResult = wasCritical ? totalBaseDamage * GetCritPower() : totalBaseDamage;
        float scaledDamageResult = damageResult * scaleFactor;

        return new DamageInfo(scaledDamageResult, wasCritical);
    }

    private float GetBaseElementalDamage(ElementalDamageType ty)
    {
        float damage = 0.0f;
        switch (ty)
        {
            case ElementalDamageType.Fire:
                {
                    damage = offensiveStats.fireDamage.GetValue();
                    break;
                }
            case ElementalDamageType.Ice:
                {
                    damage = offensiveStats.iceDamage.GetValue();
                    break;
                }
            case ElementalDamageType.Lightning:
                {
                    damage = offensiveStats.lightningDamage.GetValue();
                    break;
                }
        }
        return damage;
    }

    private float CalculateElementalDamageImpl(ElementalDamageType ty)
    {
        float basePrimaryElementalDamage = GetBaseElementalDamage(ty);

        float bonusElementalDamage = majorStats.intelligence.GetValue() * intelligenceElementalDamageMultiplier;

        float totalPrimaryDamage = basePrimaryElementalDamage > 0.0 
            ? basePrimaryElementalDamage + bonusElementalDamage
            : 0.0f;

        return totalPrimaryDamage;
    }

    public ElementalDamageInfo CalculateElementalDamage(ElementalDamageType primary, ElementalDamageType secondary = ElementalDamageType.None, float scaleFactor = 1.0f)
    {
        float primaryDamage = CalculateElementalDamageImpl(primary) * scaleFactor;
        float secondaryDamage = CalculateElementalDamageImpl(secondary) * secondaryElementMultiplier * scaleFactor;

        return new ElementalDamageInfo(primaryDamage, primary, secondaryDamage, secondary);
    }

    private float GetBaseElementalResistance(ElementalDamageType ty)
    {
        float resistance = 0.0f;
        switch (ty)
        {
            case ElementalDamageType.Fire:
                {
                    resistance = deffensiveStats.fireResistance.GetValue();
                    break;
                }
            case ElementalDamageType.Ice:
                {
                    resistance = deffensiveStats.iceResistance.GetValue();
                    break;
                }
            case ElementalDamageType.Lightning:
                {
                    resistance = deffensiveStats.lightningResistance.GetValue();
                    break;
                }
        }
        return resistance;
    }

    public float GetElementalResistance(ElementalDamageType ty)
    {
        float baseResistance = GetBaseElementalResistance(ty);
        float bonusResistance = majorStats.intelligence.GetValue() * intelligenceElementalResistMultiplier;

        return Mathf.Clamp(baseResistance + bonusResistance, 0, elementalResistanceCap) / 100.0f;
    }
}
