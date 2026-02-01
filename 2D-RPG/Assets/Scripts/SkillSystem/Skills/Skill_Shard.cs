using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardObject;

    [Header("Base Info")]
    [SerializeField] private float detonationTime;

    [Header("Move to Enemy Info")]
    [SerializeField] private float movementSpeed;

    [Header("Multicast Info")]
    [SerializeField] private int maxCharges;
    private int currentCharges;
    private bool isRecharging;

    [Header("Teleport Info")]
    [SerializeField] private float teleportShardDuration;
    [SerializeField] private float explosionDelayAfterSwap;
    private float playerHPPercentageOnCreation;

    private SkillObject_Shard currentShard;

    protected override void Awake()
    {
        base.Awake();

        currentCharges = maxCharges;
    }

    public override void TryToUseSkill()
    {
        if (!CanUseSkill()) return;

        if (IsLearned(SkillUpgradeType.Shard)) {
            ShardSkillBasic();
        }

        if (IsLearned(SkillUpgradeType.Shard_MoveToEnemy))
        {
            ShardSkillMoveToEnemy();
        }

        if (IsLearned(SkillUpgradeType.Shard_Multicast))
        {
            ShardSkillMulticast();
        }

        if (IsLearned(SkillUpgradeType.Shard_Teleport))
        {
            ShardSkillTeleport();
        }

        if (IsLearned(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            ShardSkillTeleportHpRewind();
        }
    }

    public float GetDetonationTime()
    {
        if (IsLearned(SkillUpgradeType.Shard_Teleport) || IsLearned(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            return teleportShardDuration;
        }

        return detonationTime;
    }

    public float GetSpeed()
    {
        return movementSpeed;
    }

    public bool CanMove()
    {
        return IsLearned(SkillUpgradeType.Shard_MoveToEnemy) || IsLearned(SkillUpgradeType.Shard_Multicast);
    }

    public SkillObject_Shard CreateShard(Transform target = null)
    {
        GameObject shardObj = Instantiate(shardObject, transform.position, Quaternion.identity);
        SkillObject_Shard shardSkill = shardObj.GetComponent<SkillObject_Shard>();
        shardSkill.SetupShardToExplode(this);

        if (CanMove() || target != null)
        {
            shardSkill.SetupToMoveTowardsTarget(target);
        }

        if (IsLearned(SkillUpgradeType.Shard_Teleport) || IsLearned(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            shardSkill.OnExplode += ForceOnCooldown;
        }

        return shardSkill;
    }

    private void ShardSkillBasic()
    {
        currentShard = CreateShard();
        SetSkillJustUsed();
    }

    private void ShardSkillMoveToEnemy()
    {
        currentShard = CreateShard();
        currentShard.SetupToMoveTowardsTarget();
        SetSkillJustUsed();
    }

    private void ShardSkillMulticast()
    {
        if (currentCharges <= 0) return;

        currentShard = CreateShard();
        currentShard.SetupToMoveTowardsTarget();

        --currentCharges;
        if (!isRecharging)
        {
            StartCoroutine(ShardRechargeCo());
        }
    }

    private void ShardSkillTeleport()
    {
        if (currentShard == null)
        {
            currentShard = CreateShard();
        }
        else
        {
            SwapLocationWithPlayer(currentShard.transform);
            currentShard.SetupShardToExplode(this);
            SetSkillJustUsed();
        }
    }

    private void ShardSkillTeleportHpRewind()
    {
        if (currentShard == null)
        {
            currentShard = CreateShard();
            playerHPPercentageOnCreation = playerHealth.GetCurrentHPPercentage();
        }
        else
        {
            SwapLocationWithPlayer(currentShard.transform);
            currentShard.SetupShardToExplode(this);
            playerHealth.SetHPPercentage(playerHPPercentageOnCreation);
            SetSkillJustUsed();
        }
    }

    private void ForceOnCooldown()
    {
        if (!IsOnCooldown())
        {
            SetSkillJustUsed();
            currentShard.OnExplode -= ForceOnCooldown;
        }
    }

    private IEnumerator ShardRechargeCo()
    {
        isRecharging = true;

        while (currentCharges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            ++currentCharges;
        }

        isRecharging = false;
    }
}
