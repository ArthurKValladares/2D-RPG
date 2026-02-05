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

        Vector2 wallJumpForce = player.GetWallJumpForce();
        player.ReceivePush(
            new Vector2(wallJumpForce.x * -player.FacingDirScale(), wallJumpForce.y),
            player.wallJumpNoMovementTimer,
            false
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
