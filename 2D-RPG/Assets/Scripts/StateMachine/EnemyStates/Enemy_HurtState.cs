using UnityEngine;

public class Enemy_HurtState : EnemyState
{
    public Enemy_HurtState(Enemy enemy)
        : base(enemy, "hurt")
    {
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
