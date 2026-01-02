using UnityEngine;

public class Player_WallJumpState : EntityState
{
    public Player_WallJumpState(Player player)
        : base(player, "jumpFall")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.wallJumpForce.x * -player.FacingDirScale(), player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.rb.linearVelocityY < 0.0f)
        {
            player.sm.ChangeState(player.fallState);
        } else if (player.wallDetected)
        {
            player.sm.ChangeState(player.wallSlideState);
        }
    }
}
