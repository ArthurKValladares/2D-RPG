using UnityEngine;

public class Player_AiredState : EntityState
{
    public Player_AiredState(Player player, string stateParameterName)
        : base(player, stateParameterName)
    {
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityX(player.moveInput.x * player.moveSpeed * player.inAirMoveMultiplier);
    }
}
