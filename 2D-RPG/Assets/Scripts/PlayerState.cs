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
        if (player.wallDetected) return false;
        if (player.sm.currentState == player.dashState) return false;

        return true;
    }

    override public void Update()
    {
        base.Update();

        if (player.input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            player.sm.ChangeState(player.dashState);
        }

        player.animator.SetFloat("yVelocity", player.rb.linearVelocityY);
    }
}
