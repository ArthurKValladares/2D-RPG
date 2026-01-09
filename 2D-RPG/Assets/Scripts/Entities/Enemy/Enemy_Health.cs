using UnityEngine;

public class Enemy_Health : Entity_Health
{
    Enemy enemy;
    Enemy_VFX enemyVFX;

    override protected void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        enemyVFX = GetComponent<Enemy_VFX>();
    }

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool tookDamage = base.TakeDamage(damage, damageDealer);

        if (tookDamage && (enemy.sm.currentState != enemy.deadState) && damageDealer.GetComponent<Player>()) {
            enemy.TryEnteringHurtState(damageDealer);
        }

        return tookDamage;
    }

    protected override void Die()
    {
        base.Die();

        enemyVFX.EnableAttackAlert(false);
    }
}
