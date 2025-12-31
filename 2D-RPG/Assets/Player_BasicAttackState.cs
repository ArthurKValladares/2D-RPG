using UnityEngine;

public class Player_BasicAttackState : EntityState
{
    private float attackVelocityTimer;

    private int comboIndex = 0;

    private float lastAttackEndedAt;
    public Player_BasicAttackState(Player player) 
        : base(player, "basicAttack")
    {
    }

    private void ResetComboIfNeeded()
    {
        if (comboIndex >= Player.NumBasicAttacks || (Time.time - lastAttackEndedAt) > player.comboResetTime)
        {
            comboIndex = 0;
        }
    }

    public override void Enter()
    {
        base.Enter();

        ResetComboIfNeeded();
        player.animator.SetInteger("basicAttackIndex", comboIndex);

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(player.attackVelocities[comboIndex].x * player.FacingDirScale(), player.attackVelocities[comboIndex].y);
    }

    public override void Exit()
    {
        base.Exit();

        ++comboIndex;
        lastAttackEndedAt = Time.time;
    }

    public override void Update()
    {
        base.Update();

        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0.0f)
        {
            player.SetVelocityX(0);
        }

        if (stateEnded)
        {
            player.sm.ChangeState(player.idleState);
        }
    }
}
