using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
    
    [Header("Unlock & Upgrades")]
    public int cost;
    public SkillType skillType;
    public bool learnedByDefault;
    public UpgradeData upgradeData;
}

[System.Serializable]
public class UpgradeData
{
    public UpgradeData(SkillUpgradeType ty = SkillUpgradeType.None, float cooldown = float.MaxValue)
    {
        this.upgradeType = ty;
        this.cooldown = cooldown;
    }

    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScaleData;

    public ElementalDamageType primaryElementalDamage;
    public ElementalDamageType secondaryElementalDamage;
}