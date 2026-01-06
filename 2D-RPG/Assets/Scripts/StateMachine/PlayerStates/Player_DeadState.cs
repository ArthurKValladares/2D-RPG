using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player) 
        : base(player, "dead")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.sm.SwitchOff();
        player.input.Disable();
        player.rb.simulated = false;
    }
}
