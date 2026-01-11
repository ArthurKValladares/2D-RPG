using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class EntityState
{
    protected Animator animator;
    protected string stateParameterName;

    protected float stateTimer;
    protected bool stateEnded;
    public EntityState(Animator animator, string stateParameterName) 
    {
        this.animator = animator;
        this.stateParameterName = stateParameterName;
    }

    public virtual void Enter()
    {
        animator.SetBool(stateParameterName, true);
        stateEnded = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public bool TimerDone()
    {
        return stateTimer <= 0.0f;
    }

    public void StateEnded()
    {
        stateEnded = true;
    }

    public virtual void Exit()
    {
        animator.SetBool(stateParameterName, false);
    }

    public virtual void UpdateAnimationParameters()
    {
    }

    public void SyncAttackSpeed(Entity entity)
    {
        entity.animator.SetFloat("attackSpeedMultiplier", entity.stats.offensiveStats.attackSpeedMultiplier.GetValue());
    }
}
