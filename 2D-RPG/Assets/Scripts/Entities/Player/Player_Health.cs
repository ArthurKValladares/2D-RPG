using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override HitInfo TakeDamage(AttackData attackData, Transform damageDealer)
    {
        HitInfo hitInfo = base.TakeDamage(attackData, damageDealer);

        if (hitInfo.didHit && player.sm.currentState != player.deadState)
        {
            player.TryEnteringHurtState();
        }

        return hitInfo;
    }
}
