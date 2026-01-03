using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy)
        : base(enemy, "idle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0.0f)
        {
            enemy.sm.ChangeState(enemy.moveState);
        }
    }
}
