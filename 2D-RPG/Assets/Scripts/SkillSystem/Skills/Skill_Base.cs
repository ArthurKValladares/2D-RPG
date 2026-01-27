using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player player;
    public Player_Health playerHealth;
    protected Player_SkillManager playerSkillManager;

    [Header("General Details")]
    [SerializeField] private SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    public DamageScaleData damageScaleData;
    public ElementalDamageType primaryElementalDamage;
    public ElementalDamageType secondaryElementalDamage;

    private float lastTimeUsed;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        playerHealth = GetComponentInParent<Player_Health>();
        playerSkillManager = GetComponentInParent<Player_SkillManager>();

        damageScaleData = new DamageScaleData();

        lastTimeUsed = Time.time - cooldown;
    }

    public virtual bool CanUseSkill()
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

    public void ReduceCooldownBy(float seconds)
    {
        lastTimeUsed -= seconds;
    }

    public void IncreaseCooldownBy(float seconds)
    {
        lastTimeUsed += seconds;
    }

    public void SetUpgradeType(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;
        primaryElementalDamage = upgrade.primaryElementalDamage;
        secondaryElementalDamage = upgrade.secondaryElementalDamage;
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

    protected bool IsOnCooldown()
    {
        return TimeSinceLastUsed() < cooldown;
    }

    //
    // Generic Skill Components
    //

    protected void SwapLocationWithPlayer(Transform entityToSwap)
    {
        Vector2 entityPos = entityToSwap.transform.position;
        Vector2 playerPos = player.transform.position;

        entityToSwap.transform.position = playerPos;
        player.TeleportPlayer(entityPos);
    }
}
