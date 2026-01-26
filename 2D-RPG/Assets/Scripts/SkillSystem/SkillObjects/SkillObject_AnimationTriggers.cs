using UnityEngine;

public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttack();
    }

    public void TryTerminate(int currentAttackIdx)
    {
        if (currentAttackIdx >= timeEcho.maxAttacks)
        {
            timeEcho.HandleDeath();
        }
    }
}
