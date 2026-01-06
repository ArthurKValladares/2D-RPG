using UnityEngine;

public class Player_HurtState : PlayerState
{
    public Player_HurtState(Player player)
        : base(player, "hurt")
    {
    }

    public override void Update()
    {
        base.Update();

        if (stateEnded)
        {
            player.sm.ChangeState(player.idleState);
        }
    }
}
