using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = Time.time - cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None) return false;

        if (IsOnCooldown())
        {
            // TODO: Will need a cooldown effect later
            return false;
        }

        return true;
    }

    public virtual void TryToUseSkill()
    {
    }

    public void SetSkillJustUsed()
    {
        lastTimeUsed = Time.time;
    }

    public void SetUpgradeType(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
    }

    public bool IsLearned(SkillUpgradeType upgradeTy)
    {
        return this.upgradeType == upgradeTy;
    }

    public virtual void OnStartEffect()
    {
    }

    public virtual void OnEndEffect()
    {
    }

    private float TimeSinceLastUsed()
    {
        return Time.time - lastTimeUsed;
    }

    private bool IsOnCooldown()
    {
        return TimeSinceLastUsed() < cooldown;
    }
}
