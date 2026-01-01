using UnityEngine;

public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player)
        : base(player, "wallSlide")
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();

        if (player.groundDetected)
        {
            player.sm.ChangeState(player.idleState);
            player.Flip();
        } else if (!player.wallDetected)
        {
            player.sm.ChangeState(player.fallState);
        } else if (player.input.Player.Jump.WasPressedThisFrame())
        {
            player.sm.ChangeState(player.wallJumpState);
        }
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0.0)
        {
            player.SetVelocityX(0.0f);
        } else
        {
            player.SetVelocity(0.0f, player.rb.linearVelocityY * player.wallSlideMultiplier);
        }
    }
}
