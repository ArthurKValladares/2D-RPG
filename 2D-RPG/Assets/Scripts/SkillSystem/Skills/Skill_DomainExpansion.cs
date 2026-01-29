using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    public bool InstantDomain()
    {
        return !IsLearned(SkillUpgradeType.Domain_EchoSpam) && !IsLearned(SkillUpgradeType.Domain_ShardSpam);
    }

    public void CreateDomain()
    {
        Debug.Log("Create Skill Object");
    }
}
