using UnityEngine;

public class Player_LaunchAttackState : PlayerState
{
    bool isOnWayUp;
    bool hasAttacked;

    public Player_LaunchAttackState(Player player)
        : base(player, "launchAttack")
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.launchAttackDuration;

        player.SetVelocity(player.launchAttackForce.x * player.FacingDirScale(), player.launchAttackForce.y);
        isOnWayUp = true;
        hasAttacked = false;
    }

    public override void Update()
    {
        base.Update();
        Debug.Log(player.rb.linearVelocity);

        if (player.rb.linearVelocityY < 0.0f)
        {
            isOnWayUp = false;
        }

        if (player.wallsDetected)
        {
            player.sm.ChangeState(player.wallSlideState);    
        }

        if (player.groundDetected && !isOnWayUp && !hasAttacked)
        {
            player.SetVelocityX(0.0f);
            player.animator.SetTrigger("launchAttackTrigger");
            hasAttacked = true;
        }

        if (stateEnded || TimerDone())
        {
            player.sm.ChangeState(player.idleState);
        }
    }
}