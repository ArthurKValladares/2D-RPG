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

    private void ShardSkillBasic()
    {
        CreateShard();
        SetSkillJustUsed();
    }

    private void ShardSkillMoveToEnemy()
    {
        CreateShard();
        currentShard.SetupToMoveTowardsClosestTarget(movementSpeed);
        SetSkillJustUsed();
    }

    private void ShardSkillMulticast()
    {
        if (currentCharges <= 0) return;

        CreateShard();
        currentShard.SetupToMoveTowardsClosestTarget(movementSpeed);

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
            CreateShard();
        }
        else
        {
            SwapLocationWithPlayer(currentShard.transform);
            currentShard.SetupShardToExplode(explosionDelayAfterSwap);
            SetSkillJustUsed();
        }
    }

    private void ShardSkillTeleportHpRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            playerHPPercentageOnCreation = playerHealth.GetCurrentHPPercentage();
        }
        else
        {
            SwapLocationWithPlayer(currentShard.transform);
            currentShard.SetupShardToExplode(explosionDelayAfterSwap);
            playerHealth.SetHPPercentage(playerHPPercentageOnCreation);
            SetSkillJustUsed();
        }
    }

    private float GetDetonationTime()
    {
        if (IsLearned(SkillUpgradeType.Shard_Teleport) || IsLearned(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            return teleportShardDuration;
        }

        return detonationTime;
    }

    private void CreateShard()
    {
        GameObject shardObj = Instantiate(shardObject, transform.position, Quaternion.identity);
        currentShard = shardObj.GetComponent<SkillObject_Shard>();

        currentShard.SetupShardToExplode(GetDetonationTime());

        if (IsLearned(SkillUpgradeType.Shard_Teleport) || IsLearned(SkillUpgradeType.Shard_TeleportHpRewind))
        {
           currentShard.OnExplode += ForceOnCooldown;
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
