using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player) 
        : base(player, "jumpFall")
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.groundDetected)
        {
            player.sm.ChangeState(player.idleState);
        } else if (player.wallDetected)
        {
            player.sm.ChangeState(player.wallSlideState);
        }
    }
}
