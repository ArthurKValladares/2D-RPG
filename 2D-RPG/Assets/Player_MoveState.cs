using UnityEngine;

public class Player_MoveState : EntityState
{
    public Player_MoveState(Player player, StateMachine sm) 
        : base(player, sm, "Move State")
    {
    }

    public override void Update()
    {
        base.Update();

        player.rb.linearVelocityX = player.moveInput.x * player.moveSpeed;
        player.rb.linearVelocityY = player.moveInput.y * player.moveSpeed;

        if (player.moveInput.x == 0.0f && player.moveInput.y == 0.0f)
        {
            sm.ChangeState(player.idleState);
        } 
    }
}
