using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [Header("Base Values")]
    public Stat<int> baseHP;

    [Header("Stats")]
    public Stat<int> vitality;

    [Header("Stats Multipliers")]
    public int vitalityHealthMultiplier;

    public int CalculateMaxHP()
    {
        return baseHP.GetValue() + vitality.GetValue() * vitalityHealthMultiplier;
    }
}
