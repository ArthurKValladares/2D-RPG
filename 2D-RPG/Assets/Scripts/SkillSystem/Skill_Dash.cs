using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public override void OnStartEffect()
    {
        base.OnStartEffect();

        if (IsLearned(SkillUpgradeType.Dash_CloneOnStart) || IsLearned(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsLearned(SkillUpgradeType.Dash_ShardOnShart) || IsLearned(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    public override void OnEndEffect()
    {
        base.OnEndEffect();

        if (IsLearned(SkillUpgradeType.Dash_CloneOnStartAndArrival))
        {
            CreateClone();
        }

        if (IsLearned(SkillUpgradeType.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        Debug.Log("Shard Created");
    }

    private void CreateClone()
    {
        Debug.Log("Clone Created");
    }
}
