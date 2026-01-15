using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }
    public Skill_Shard shard { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
        shard = GetComponentInChildren<Skill_Shard>();
    }

    public Skill_Base GetSkillByType(SkillType ty)
    {
        switch(ty)
        {
            case SkillType.Dash: return dash;
            case SkillType.Shard: return shard;
            default: 
            {
                Debug.Log("Skill not implemented: " + ty);
                return null;
            }
        }
    }
}
