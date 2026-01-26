using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float duration;

    [Header("Attack Upgrades")]
    [SerializeField] public int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;
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

    public void CreateTimeEcho(Vector3? position = null)
    {
        Vector3 echoPosition = position ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, echoPosition, Quaternion.identity);
        SkillObject_TimeEcho timeEchoObj = timeEcho.GetComponent<SkillObject_TimeEcho>();
        timeEchoObj.SetupTimeEcho(this);
    }
}
