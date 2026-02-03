using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainExpansionPrefab;

    [Header("Core Details")]
    [SerializeField] public float maximumSizeScale = 10.0f;
    [SerializeField] public float transformationDuration = 0.2f;

    [Header("Slow Down/Base Upgrade")]
    [SerializeField] private float baseDomainDuration = 5.0f;
    [SerializeField] private float baseSlowDownPercentage = 0.7f;
    [SerializeField] private Color baseColor;

    [Header("Shard Upgrade")]
    [SerializeField] private float shardDomainDuration = 5.0f;
    [SerializeField] private float shardSlowDownPercentage = 0.7f;
    [SerializeField] private int shardsToCast = 10;
    [SerializeField] private Color shardColor;

    [Header("Echo Upgrade")]
    [SerializeField] private float echoDomainDuration = 5.0f;
    [SerializeField] private float echoSlowDownPercentage = 0.7f;
    [SerializeField] private int echosToCast = 10;
    [SerializeField] private Color echoColor;

    private float spellTimer;
    private float spellsPerSecond;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public bool InstantDomain()
    {
        return !IsLearned(SkillUpgradeType.Domain_EchoSpam) && !IsLearned(SkillUpgradeType.Domain_ShardSpam);
    }

    public void CreateDomain()
    {
        GameObject domain = Instantiate(domainExpansionPrefab, transform.position, Quaternion.identity);
        SkillObject_DomainExpansion domainObject = domain.GetComponent<SkillObject_DomainExpansion>();
        domainObject.SetupDomain(this);

        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();
    }

    public float GetDomainDuration()
    {
        if (IsLearned(SkillUpgradeType.Domain_SlowDown))
        {
            return baseDomainDuration;
        } else if (IsLearned(SkillUpgradeType.Domain_ShardSpam))
        {
            return shardDomainDuration;
        } else if (IsLearned(SkillUpgradeType.Domain_EchoSpam))
        {
            return echoDomainDuration;
        }
        Debug.LogError("Did not implement GetDuration for upgrade of type: " + upgradeType);
        return baseDomainDuration;
    }

    public float GetSlowDownPercentage()
    {
        if (IsLearned(SkillUpgradeType.Domain_SlowDown))
        {
            return baseSlowDownPercentage;
        }
        else if (IsLearned(SkillUpgradeType.Domain_ShardSpam))
        {
            return shardSlowDownPercentage;
        }
        else if (IsLearned(SkillUpgradeType.Domain_EchoSpam))
        {
            return echoSlowDownPercentage;
        }
        Debug.LogError("Did not implement GetSlowDownPercentage for upgrade of type: " + upgradeType);
        return baseSlowDownPercentage;
    }

    public Color GetDomainColor()
    {
        if (IsLearned(SkillUpgradeType.Domain_SlowDown))
        {
            return baseColor;
        }
        else if (IsLearned(SkillUpgradeType.Domain_ShardSpam))
        {
            return shardColor;
        }
        else if (IsLearned(SkillUpgradeType.Domain_EchoSpam))
        {
            return echoColor;
        }
        Debug.LogError("Did not implement GetDomainColor for upgrade of type: " + upgradeType);
        return Color.white;
    }

    private int GetSpellsToCast()
    {
        if (IsLearned(SkillUpgradeType.Domain_ShardSpam))
        {
            return shardsToCast;
        }
        else if (IsLearned(SkillUpgradeType.Domain_EchoSpam))
        {
            return echosToCast;
        }

        return 0;
    }

    public void DoSpellCasting()
    {
        spellTimer -= Time.deltaTime;

        if (currentTarget == null)
        {
            currentTarget = FindRandomTargetInDomain();
        }

        if (currentTarget != null && spellTimer <= 0.0f)
        {
            CastSpell(currentTarget);

            spellTimer = 1.0f / spellsPerSecond;
            currentTarget = null;
        }
    }

    public void AddTarget(Enemy enemy)
    {
        trappedTargets.Add(enemy);
    }

    public void ClearTargets()
    {
        foreach (Enemy enemy in trappedTargets)
        {
            enemy.StopSlowDown();
        }

        trappedTargets.Clear();
    }

    private Transform FindRandomTargetInDomain()
    {
        trappedTargets.RemoveAll(enemy => enemy == null || enemy.health.currentHP <= 0);

        if (trappedTargets.Count == 0) return null;

        int idx = Random.Range(0, trappedTargets.Count);
        Enemy enemy = trappedTargets[idx];

        return enemy.transform;
    }

    private void CastSpell(Transform target)
    {
        if (IsLearned(SkillUpgradeType.Domain_ShardSpam))
        {
            SkillObject_Shard shard = player.skillManager.shard.CreateShard(currentTarget);
        }
        else if (IsLearned(SkillUpgradeType.Domain_EchoSpam))
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1.0f, 0.0f) : new Vector2(-1.0f, 0.0f);

            player.skillManager.timeEcho.CreateTimeEcho(currentTarget.position + offset);
        }
    }
}
