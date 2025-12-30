using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine sm;
    protected string stateName;

    public EntityState(Player player, StateMachine sm, string stateName) 
    {
        this.player = player;
        this.sm = sm;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }
}
