using UnityEngine;

public class Player_SkillManager : MonoBehaviour
{
    public Skill_Dash dash { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<Skill_Dash>();
    }

    public Skill_Base GetSkillByType(SkillType ty)
    {
        switch(ty)
        {
            case SkillType.Dash: return dash;
            default: 
            {
                Debug.Log("Skill not implemented: " + ty);
                return null;
            }
        }
    }
}
