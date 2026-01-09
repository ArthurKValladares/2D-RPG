using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override bool TakeDamage(float physicalDamage, ElementalDamageInfo elementalDamage, Transform damageDealer)
    {
        bool tookDamage = base.TakeDamage(physicalDamage, elementalDamage, damageDealer);

        if (tookDamage && player.sm.currentState != player.deadState)
        {
            player.TryEnteringHurtState();
        }

        return tookDamage;
    }
}
