using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, string stateParameterName) 
        : base(enemy.animator, stateParameterName)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();    
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        animator.SetFloat("moveAnimMultiplier", enemy.moveSpeedMultiplier);
        animator.SetFloat("battleAnimMultiplier", enemy.battleMoveSpeed / enemy.moveSpeed);
        animator.SetFloat("xVelocity", enemy.rb.linearVelocityX);
    }
}
