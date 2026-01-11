using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy)
        : base(enemy, "attack")
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SyncAttackSpeed();
    }

    public override void Update()
    {
        base.Update();

        if (stateEnded)
        {
            enemy.sm.ChangeState(enemy.battleState);
        }
    }
}
