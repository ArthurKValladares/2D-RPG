using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (player.sm.currentState != player.deadState)
        {
            player.TryEnteringHurtState();
        }
    }
}
