using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy)
        : base(enemy, "move")
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!enemy.groundDetected || enemy.wallsDetected)
        {
            enemy.Flip();
        }
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocityX(enemy.moveSpeed * enemy.FacingDirScale());

        if (!enemy.groundDetected || enemy.wallsDetected)
        {
            enemy.sm.ChangeState(enemy.idleState);
        }
    }
}
