using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    public Entity_Combat combat;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();
    }

    private void CurrentStateEnded()
    {
        entity.SetCurrentStateEnded();
    }

    private void AttackTrigger()
    {
        combat.PerformAttack();
    }
}
