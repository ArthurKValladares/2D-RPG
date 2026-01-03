using UnityEngine;

public class Player_DashState : PlayerState
{
    public Player_DashState(Player player)
        : base(player, "dash")
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashTime;
        player.rb.gravityScale = 0.0f;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0.0f, 0.0f);
        player.rb.gravityScale = player.originalGravityscale;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashForce * player.FacingDirScale(), 0.0f);

        if (stateTimer <= 0.0f)
        {
            if (player.groundDetected)
            {
                player.sm.ChangeState(player.idleState);
            } else
            {
                player.sm.ChangeState(player.fallState);
            }
        } else
        {
            CancelDashIfNeeded();
        }
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallsDetected)
        {
            if (player.groundDetected)
            {
                player.sm.ChangeState(player.idleState);
            }
            else
            {
                player.sm.ChangeState(player.wallSlideState);
            }
        }
    }
}
