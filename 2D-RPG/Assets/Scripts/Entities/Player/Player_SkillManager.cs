using UnityEngine;

// TODO: This file can be generated
public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }
    public Skill_SwordThrow swordThrow { get; private set; }
    public Skill_TimeEcho timeEcho { get; private set; }

    private Skill_Base[] allSkills;

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
        swordThrow = GetComponentInChildren<Skill_SwordThrow>();
        timeEcho = GetComponentInChildren<Skill_TimeEcho>();

        allSkills = GetComponentsInChildren<Skill_Base>();
    }

    public Skill_Base GetSkillByType(SkillType ty)
    {
        switch(ty)
        {
            case SkillType.Dash: return dash;
            case SkillType.Shard: return shard;
            case SkillType.SwordThrow: return swordThrow;
            case SkillType.TimeEcho: return timeEcho;
            default: 
            {
                Debug.Log("Skill not implemented: " + ty);
                return null;
            }
        }
    }

    public void ReduceAllCooldownsBy(float seconds)
    {
        foreach(Skill_Base skill in allSkills)
        {
            skill.ReduceCooldownBy(seconds);
        }
    }
}
