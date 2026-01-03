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

        animator.SetFloat("moveAnimMultiplier", enemy.moveSpeedMultiplier);
    }
}
