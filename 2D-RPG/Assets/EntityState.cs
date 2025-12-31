using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected string stateParameterName;

    protected float stateTimer;
    protected bool stateEnded;
    public EntityState(Player player, string stateParameterName) 
    {
        this.player = player;
        this.stateParameterName = stateParameterName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(stateParameterName, true);
        stateEnded = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        if (player.input.Player.Dash.WasPressedThisFrame())
        {
            player.sm.ChangeState(player.dashState);
        }

        player.animator.SetFloat("yVelocity", player.rb.linearVelocityY);
    }

    public void StateEnded()
    {
        stateEnded = true;
    }

    public virtual void Exit()
    {
        player.animator.SetBool(stateParameterName, false);
    }
}
