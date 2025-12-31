using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player) 
        : base(player, "walk")
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0.0f || player.wallDetected)
        {
            player.sm.ChangeState(player.idleState);
        }

        player.SetVelocityX(player.moveInput.x * player.moveSpeed);
    }
}
