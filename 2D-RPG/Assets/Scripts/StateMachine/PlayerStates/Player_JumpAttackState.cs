using UnityEngine;

public class Player_JumpAttackState : Player_AiredState
{
    public Player_JumpAttackState(Player player) 
        : base(player, "jumpAttack")
    {
    }

    public override void Update()
    {
        base.Update();

        if (stateEnded)
        {
            if (player.groundDetected)
            {
                player.sm.ChangeState(player.idleState);
            }
            else if (player.wallsDetected)
            {
                player.sm.ChangeState(player.wallSlideState);
            } else
            {
                player.sm.ChangeState(player.fallState);
            }
        }
    }
}
