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

        player.ReceivePush(
            new Vector2(player.wallJumpForce.x * -player.FacingDirScale(), player.wallJumpForce.y),
            player.wallJumpNoMovementTimer
        );
    }

    public override void Update()
    {
        base.Update();

        if (player.rb.linearVelocityY < 0.0f)
        {
            player.sm.ChangeState(player.fallState);
        } else if (player.wallsDetected)
        {
            player.sm.ChangeState(player.wallSlideState);
        }
    }
}
