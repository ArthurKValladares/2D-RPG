using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float duration;

    [Header("Attack Upgrades")]
    [SerializeField] public int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;

    [Header("Wisp Upgrades")]
    [SerializeField] public float wispMoveSpeed = 15.0f;
    [SerializeField] private float damagePercentHealed = 0.3f;
    [SerializeField] private float cooldownReducedInSeconds = 1.0f;
    [SerializeField] private Color healWispColor;
    [SerializeField] private Color cleanseWispColor;
    [SerializeField] private Color cooldownWispColor;

    public override void TryToUseSkill()
    {
        if (CanUseSkill())
        {
            CreateTimeEcho();
        }
    }

    public float GetEchoDuration()
    {
        return duration;
    }

    public int GetMaxAttacks()
    {
        if (IsLearned(SkillUpgradeType.TimeEcho_MultiAttack))
        {
            return maxAttacks;
        }

        if (IsLearned(SkillUpgradeType.TimeEcho_SingleAttack) || IsLearned(SkillUpgradeType.TimeEcho_ChanceToMultiply))
        {
            return 1;
        }

        return 0;
    }

    public float GetDuplicateChance()
    {
        if(!IsLearned(SkillUpgradeType.TimeEcho_ChanceToMultiply))
        {
            return 0.0f;
        }

        return duplicateChance;
    }

    public bool ShouldBeWisp()
    {
        return IsLearned(SkillUpgradeType.TimeEcho_HealWisp) 
            || IsLearned(SkillUpgradeType.TimeEcho_CleanseWisp)
            || IsLearned(SkillUpgradeType.TimeEcho_CooldownWisp);
    }

    public Color GetWispColor()
    {
        if (IsLearned(SkillUpgradeType.TimeEcho_HealWisp))
        {
            return healWispColor;
        }
        else if (IsLearned(SkillUpgradeType.TimeEcho_CleanseWisp))
        {
            return cleanseWispColor;
        }
        else if (IsLearned(SkillUpgradeType.TimeEcho_CooldownWisp))
        {
            return cooldownWispColor;
        }
        return Color.white;
    }

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp())
        {
            return damagePercentHealed;
        }

        return 0.0f;
    }

    public float GetCooldownReductionInSeconds()
    {
        if (IsLearned(SkillUpgradeType.TimeEcho_CooldownWisp))
        {
            return cooldownReducedInSeconds;
        }

        return 0.0f;
    }

    public bool CanRemoveNegativeEffects()
    {
        if (IsLearned(SkillUpgradeType.TimeEcho_CleanseWisp))
        {
            return true;
        }

        return false;
    }

    public void CreateTimeEcho(Vector3? position = null)
    {
        Vector3 echoPosition = position ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, echoPosition, Quaternion.identity);
        SkillObject_TimeEcho timeEchoObj = timeEcho.GetComponent<SkillObject_TimeEcho>();
        timeEchoObj.SetupTimeEcho(this);
    }
}
