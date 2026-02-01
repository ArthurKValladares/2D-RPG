using UnityEngine;

public class PlayerState : EntityState
{
    protected Player player;

    public PlayerState(Player player, string stateParameterName)
        : base(player.animator, stateParameterName)
    {
        this.player = player;
    }

    private bool CanDash()
    {
        if (player.wallsDetected) return false;
        if (player.sm.currentState == player.dashState || player.sm.currentState == player.domainExpansionState) return false;
        if (!player.skillManager.dash.CanUseSkill()) return false;

        return true;
    }

    override public void Update()
    {
        base.Update();

        // TODO: Dash should be handled like shard
        if (player.input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            player.skillManager.dash.SetSkillJustUsed();
            player.sm.ChangeState(player.dashState);
        }

        if (player.input.Player.UltimateSpell.WasPressedThisFrame() && player.skillManager.domainExpansion.CanUseSkill())
        {
            if (player.skillManager.domainExpansion.InstantDomain())
            {
                player.skillManager.domainExpansion.CreateDomain();
            } else
            {
                player.sm.ChangeState(player.domainExpansionState);
            }

            player.skillManager.domainExpansion.SetSkillJustUsed();
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        player.animator.SetFloat("yVelocity", player.rb.linearVelocityY);
    }
}
