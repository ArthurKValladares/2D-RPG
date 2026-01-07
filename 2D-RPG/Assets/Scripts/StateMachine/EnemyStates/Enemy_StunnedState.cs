using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX enemyVFX;

    public Enemy_StunnedState(Enemy enemy)
        : base(enemy, "stunned")
    {
        enemyVFX = enemy.GetComponent<Enemy_VFX>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.stunnedDuration;
        
        enemyVFX.EnableAttackAlert(false);

        enemy.EnableCounterWindow(false);
        enemy.rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.FacingDirScale(), enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (TimerDone())
        {
            enemy.sm.ChangeState(enemy.idleState);
        }
    }
}
