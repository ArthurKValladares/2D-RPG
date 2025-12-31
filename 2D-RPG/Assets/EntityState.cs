using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected string stateParameterName;

    public EntityState(Player player, string stateParameterName) 
    {
        this.player = player;
        this.stateParameterName = stateParameterName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(stateParameterName, true);
    }

    public virtual void Update()
    {
        player.animator.SetFloat("yVelocity", player.rb.linearVelocityY);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(stateParameterName, false);
    }
}
