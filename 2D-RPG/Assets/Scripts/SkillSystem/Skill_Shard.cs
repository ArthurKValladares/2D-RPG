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

    private void CreateShard()
    {
        GameObject shardObj = Instantiate(shardObject, transform.position, Quaternion.identity);
        currentShard = shardObj.GetComponent<SkillObject_Shard>();

        currentShard.SetupShardToExplode(detonationTime);
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
