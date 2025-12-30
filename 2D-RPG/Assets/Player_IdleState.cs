using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(Player player, StateMachine sm)
        : base(player, sm, "Idle State")
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0.0f || player.moveInput.y != 0.0f)
        {
            sm.ChangeState(player.moveState);
        }
    }
}
