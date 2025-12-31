using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player) 
        : base(player, "jumpFall")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.linearVelocityY = player.jumpForce;
    }

    public override void Update()
    {
        base.Update();

        if (player.rb.linearVelocityY <= 0.0)
        {
            player.sm.ChangeState(player.fallState);
        }
    }
}
