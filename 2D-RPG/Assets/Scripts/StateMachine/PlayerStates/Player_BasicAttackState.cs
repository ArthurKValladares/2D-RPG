using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private int comboIndex = 0;
    private float lastAttackEndedAt;
    private bool nextAttackQueued;

    public Player_BasicAttackState(Player player) 
        : base(player, "basicAttack")
    {
    }

    private void ResetComboIfNeeded()
    {
        if (IsLastAttackInChain() || (Time.time - lastAttackEndedAt) > player.comboResetTime)
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
        player.SetVelocity(player.attackVelocities[comboIndex].x * AttackDirScale(), player.attackVelocities[comboIndex].y);

        nextAttackQueued = false;
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

        if (player.input.Player.Attack.WasPressedThisFrame() && !IsLastAttackInChain())
        {
            nextAttackQueued = true;
        }

        if (stateEnded)
        {
            if (nextAttackQueued)
            {
                player.animator.SetBool(stateParameterName, false);
                player.EnterAttackStateWithDelay();
            } else
            {
                player.sm.ChangeState(player.idleState);
            }
        }
    }

    private float AttackDirScale()
    {
        return player.moveInput.x != 0
            ? player.moveInput.x
            : player.FacingDirScale();
    }

    private bool IsLastAttackInChain()
    {
        return comboIndex >= Player.NumBasicAttacks;
    }
}
