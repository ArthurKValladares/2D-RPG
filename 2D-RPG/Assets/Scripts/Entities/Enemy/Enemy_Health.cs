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

    public override HitInfo TakeDamage(float physicalDamage, ElementalDamageInfo elementalDamage, Transform damageDealer)
    {
        HitInfo hitInfo = base.TakeDamage(physicalDamage, elementalDamage, damageDealer);

        if (hitInfo.didHit && (enemy.sm.currentState != enemy.deadState) && damageDealer.GetComponent<Player>()) {
            enemy.TryEnteringHurtState(damageDealer);
        }

        return hitInfo;
    }

    protected override void Die()
    {
        base.Die();

        enemyVFX.EnableAttackAlert(false);
    }
}
