using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;

    public bool HasEnoughSkillPoints(int points)
    {
        return skillPoints >= points;
    }

    public void RemoveSkillPoints(int points)
    {
        if (skillPoints < points)
        {
            Debug.LogWarning("Not enough skill points. Requires " + points + " has " + skillPoints);
        }

        skillPoints = Mathf.Max(0, skillPoints - points);
    }
}
