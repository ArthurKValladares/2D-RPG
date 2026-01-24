using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    protected override void Die()
    {
        base.Die();

        // TODO: This is very sloppy and hard-coded for now
        SkillObject_TimeEcho timeEcho = GetComponent<SkillObject_TimeEcho>();
        timeEcho.HandleDeath();
    }
}
