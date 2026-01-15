using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")]
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = Time.time - cooldown;
    }

    public bool CanUseSkill()
    {
        if (IsOnCooldown())
        {
            Debug.Log("On Cooldown: Used " + TimeSinceLastUsed() + "s ago. Cooldown " + cooldown + "s");
            return false;
        }

        return true;
    }

    private float TimeSinceLastUsed()
    {
        return Time.time - lastTimeUsed;
    }

    private bool IsOnCooldown()
    {
        return (Time.time - lastTimeUsed) < cooldown;
    }

    public void SetSkillJustUsed()
    {
        lastTimeUsed = Time.time;
    }

    private void ReduceCooldownBy(float time)
    {
        lastTimeUsed -= time;
    }
    private void IncreaseCooldownBy(float time)
    {
        lastTimeUsed += time;
    }
}
