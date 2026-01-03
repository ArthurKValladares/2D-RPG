using UnityEngine;

public class Player_AiredState : PlayerState
{
    protected bool canMove = true;

    public Player_AiredState(Player player, string stateParameterName)
        : base(player, stateParameterName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (canMove)
        {
            player.SetVelocityX(player.moveInput.x * player.moveSpeed * player.inAirMoveMultiplier);
        }

        if (player.input.Player.Attack.WasPressedThisFrame() && player.sm.currentState != player.jumpAttackState)
        {
            player.sm.ChangeState(player.jumpAttackState);
        }
    }
}
