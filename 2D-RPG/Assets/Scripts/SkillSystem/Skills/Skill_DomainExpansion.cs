using Unity.Cinemachine;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainExpansionPrefab;

    [Header("Core Details")]
    [SerializeField] public float maximumSizeScale = 10.0f;
    [SerializeField] public float transformationDuration = 0.2f;

    [Header("Slow Down/Base Upgrade")]
    [SerializeField] private float baseDomainDuration = 5.0f;
    [SerializeField] private float baseSlowDownPercentage = 0.9f;

    [Header("Shard Upgrade")]
    [SerializeField] private float shardDomainDuration = 5.0f;
    [SerializeField] private float shardSlowDownPercentage = 0.9f;

    [Header("Echo Upgrade")]
    [SerializeField] private float echoDomainDuration = 5.0f;
    [SerializeField] private float echoSlowDownPercentage = 0.9f;

    public bool InstantDomain()
    {
        return !IsLearned(SkillUpgradeType.Domain_EchoSpam) && !IsLearned(SkillUpgradeType.Domain_ShardSpam);
    }

    public void CreateDomain()
    {
        GameObject domain = Instantiate(domainExpansionPrefab, transform.position, Quaternion.identity);
        SkillObject_DomainExpansion domainObject = domain.GetComponent<SkillObject_DomainExpansion>();
        domainObject.SetupDomain(this);
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
}
