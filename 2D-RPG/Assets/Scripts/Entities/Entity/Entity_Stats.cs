using UnityEngine;

public class PhysicalDamageInfo
{
    public PhysicalDamageInfo(float damageResult, bool wasCritical)
    {
        this.damageResult = damageResult;
        this.wasCritical = wasCritical;
    }

    public float damageResult;
    public bool wasCritical;
};

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] private Stat_SetupSO defaultStatSetup;

    [Header("All Percentage values should be in the 0-100 range, not 0-1.")]

    [Header("Stats")]
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offensive;
    public Stat_DefensiveGroup deffensive;
    public Stat_ResourceGroup resources;

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
    public float elementalResistanceCap = 75.0f;

    public float CalculateMaxHP()
    {
        float baseHP = resources.maxHealth.GetValue();
        float bonusHP = major.vitality.GetValue() * vitalityHealthMultiplier;

        return baseHP + bonusHP;
    }

    public float CalculateEvasion()
    {
        float baseEvasion = deffensive.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * agilityEvasionMultiplier;
        float clampedEvasion = Mathf.Clamp(baseEvasion + bonusEvasion, 0.0f, maxEvasion);
        
        return clampedEvasion / 100.0f;
    }

    private float GetArmor()
    {
        float baseArmor = deffensive.armor.GetValue();
        float bonusArmor = major.vitality.GetValue() * vitalityArmorMultiplier;

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
        return offensive.armorReduction.GetValue() / 100.0f;
    }

    private float GetCritPower()
    {
        float baseCritPower = offensive.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * strengthCritPowerMultiplier;

        return (baseCritPower + bonusCritPower) / 100.0f;
    }

    private float GetCritChance()
    {
        float baseCritChance = offensive.critChance.GetValue();
        float bonuesritChange = major.agility.GetValue() * agilityCritChanceMultiplier;

        return (baseCritChance + bonuesritChange) / 100.0f;
    }

    public PhysicalDamageInfo CalculatePhysicalDamage(float scaleFactor)
    {
        float baseDamage = offensive.physicalDamage.GetValue();
        float bonusDamage = major.strength.GetValue() * strengthDamageMultiplier;
        float totalBaseDamage = baseDamage + bonusDamage;

        bool wasCritical = Random.Range(0.0f, 1.0f) < GetCritChance();

        float damageResult = wasCritical ? totalBaseDamage * GetCritPower() : totalBaseDamage;
        float scaledDamageResult = damageResult * scaleFactor;

        return new PhysicalDamageInfo(scaledDamageResult, wasCritical);
    }

    private float GetBaseElementalDamage(ElementalDamageType ty)
    {
        float damage = 0.0f;
        switch (ty)
        {
            case ElementalDamageType.Fire:
                {
                    damage = offensive.fireDamage.GetValue();
                    break;
                }
            case ElementalDamageType.Ice:
                {
                    damage = offensive.iceDamage.GetValue();
                    break;
                }
            case ElementalDamageType.Lightning:
                {
                    damage = offensive.lightningDamage.GetValue();
                    break;
                }
        }
        return damage;
    }

    private float CalculateElementalDamageImpl(ElementalDamageType ty)
    {
        if (ty == ElementalDamageType.None) return 0.0f;

        float basePrimaryElementalDamage = GetBaseElementalDamage(ty);

        float bonusElementalDamage = major.intelligence.GetValue() * intelligenceElementalDamageMultiplier;

        float totalPrimaryDamage = basePrimaryElementalDamage > 0.0 
            ? basePrimaryElementalDamage + bonusElementalDamage
            : 0.0f;

        return totalPrimaryDamage;
    }

    public ElementalDamageInfo CalculateElementalDamage(ElementalDamageType primary, ElementalDamageType secondary, float secondaryElementMultiplier, float scaleFactor)
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
                    resistance = deffensive.fireResistance.GetValue();
                    break;
                }
            case ElementalDamageType.Ice:
                {
                    resistance = deffensive.iceResistance.GetValue();
                    break;
                }
            case ElementalDamageType.Lightning:
                {
                    resistance = deffensive.lightningResistance.GetValue();
                    break;
                }
        }
        return resistance;
    }

    public float GetElementalResistance(ElementalDamageType ty)
    {
        float baseResistance = GetBaseElementalResistance(ty);
        float bonusResistance = major.intelligence.GetValue() * intelligenceElementalResistMultiplier;

        return Mathf.Clamp(baseResistance + bonusResistance, 0, elementalResistanceCap) / 100.0f;
    }

    public Stat GetStat(StatType ty)
    {
        switch (ty)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegenPerSecond;
            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;
            case StatType.AttackSpeed: return offensive.attackSpeedMultiplier;
            case StatType.Damage: return offensive.physicalDamage;
            case StatType.CritChance: return offensive.critChance;
            case StatType.CritPower: return offensive.critPower;
            case StatType.ArmorReduction: return offensive.armorReduction;
            case StatType.FireDamage: return offensive.fireDamage;
            case StatType.IceDamage: return offensive.iceDamage;
            case StatType.LightningDamage: return offensive.lightningDamage;
            case StatType.Armor: return deffensive.armor;
            case StatType.Evasion: return deffensive.evasion;
            case StatType.IceResistance: return deffensive.iceResistance;
            case StatType.FireResistance: return deffensive.fireResistance;
            case StatType.LightningResistance: return deffensive.lightningResistance;
            default: 
            {
                Debug.LogError($"StatType {ty} not implemented");
                return null;
            }
        };
    }

    [ContextMenu("Update Default Stat Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log("No default stat setup assigned");
            return;
        }

        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegenPerSecond.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offensive.attackSpeedMultiplier.SetBaseValue(defaultStatSetup.attackSpeedMultiplier);
        offensive.physicalDamage.SetBaseValue(defaultStatSetup.damage);
        offensive.critChance.SetBaseValue(defaultStatSetup.critChance);
        offensive.critPower.SetBaseValue(defaultStatSetup.critPower);
        offensive.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);

        offensive.iceDamage.SetBaseValue(defaultStatSetup.iceDamage);
        offensive.fireDamage.SetBaseValue(defaultStatSetup.fireDamage);
        offensive.lightningDamage.SetBaseValue(defaultStatSetup.lightningDamage);

        deffensive.armor.SetBaseValue(defaultStatSetup.armor);
        deffensive.evasion.SetBaseValue(defaultStatSetup.evasion);

        deffensive.iceResistance.SetBaseValue(defaultStatSetup.iceResistance);
        deffensive.fireResistance.SetBaseValue(defaultStatSetup.fireResistance);
        deffensive.lightningResistance.SetBaseValue(defaultStatSetup.lightningResistance);
    }
}
