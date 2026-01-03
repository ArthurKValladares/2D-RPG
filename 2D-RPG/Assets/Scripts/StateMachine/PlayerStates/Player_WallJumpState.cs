using UnityEngine;

public class Player_WallJumpState : Player_AiredState
{
    public Player_WallJumpState(Player player)
        : base(player, "jumpFall")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.wallJumpForce.x * -player.FacingDirScale(), player.wallJumpForce.y);

        stateTimer = player.wallJumpNoMovementTimer;
        canMove = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0.0f)
        {
            canMove = true;
        }

        if (player.rb.linearVelocityY < 0.0f)
        {
            player.sm.ChangeState(player.fallState);
        } else if (player.wallsDetected)
        {
            player.sm.ChangeState(player.wallSlideState);
        }
    }
}
