using UnityEngine;

public class Player_ParryState : PlayerState
{
    private Player_Combat combat;
    private bool parryPerformed;

    public Player_ParryState(Player player)
        : base(player, "parry")
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.parryRecoveryDuration;

        SetParryPerformed(combat.PerformParry());
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityX(0);

        if (stateEnded)
        {
            player.sm.ChangeState(player.idleState);
        }

        if (TimerDone() && !parryPerformed)
        {
            player.sm.ChangeState(player.idleState);
        }
    }

    private void SetParryPerformed(bool performed)
    {
        parryPerformed = performed;
        player.animator.SetBool("parryPerformed", performed);
    }
}
