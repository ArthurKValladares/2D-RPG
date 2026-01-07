using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, string stateParameterName) 
        : base(player, stateParameterName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.rb.linearVelocityY < 0.0f && !player.groundDetected)
        {
            player.sm.ChangeState(player.fallState);
        }
        if (player.input.Player.Jump.WasPressedThisFrame())
        {
            player.rb.linearVelocityY = player.jumpForce;

            player.sm.ChangeState(player.jumpState);
        } else if (player.input.Player.Attack.WasPressedThisFrame())
        {
            player.sm.ChangeState(player.basicAttackState);
        } else if (player.input.Player.LaunchAttack.WasPressedThisFrame())
        {
            player.sm.ChangeState(player.launchAttackState);
        } else if (player.input.Player.Parry.WasPressedThisFrame())
        {
            player.sm.ChangeState(player.parryState);
        }
    }
}
