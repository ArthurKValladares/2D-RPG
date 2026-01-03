using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, string stateParameterName)
        : base(enemy, stateParameterName)
    {
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D hit = enemy.PlayerDetection();
        if (hit)
        {
            enemy.sm.ChangeState(enemy.battleState);
        }
    }
}
