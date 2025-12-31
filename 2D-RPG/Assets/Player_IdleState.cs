using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player)
        : base(player, "idle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityX(0.0f);
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == player.FacingDirScale() && player.wallDetected) return;

        if (player.moveInput.x != 0.0f)
        {
            player.sm.ChangeState(player.moveState);
        }
    }
}
