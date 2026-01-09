using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool tookDamage = base.TakeDamage(damage, damageDealer);

        if (tookDamage && player.sm.currentState != player.deadState)
        {
            player.TryEnteringHurtState();
        }

        return tookDamage;
    }
}
