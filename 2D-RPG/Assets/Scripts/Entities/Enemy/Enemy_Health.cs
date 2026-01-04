using UnityEngine;

public class Enemy_Health : Entity_Health
{
    Enemy enemy;

    override protected void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (damageDealer.GetComponent<Player>()) {
            enemy.TryEnteringBattleState(damageDealer);
        }
    }
}
