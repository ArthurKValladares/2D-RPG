using UnityEngine;

public class Player_LaunchAttackState : PlayerState
{
    private bool touchedGround;
    public Player_LaunchAttackState(Player player)
        : base(player, "launchAttack")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.launchAttackForce.x * player.FacingDirScale(), player.launchAttackForce.y);
        touchedGround = false;
    }

    public override void Update()
    {
        base.Update();

        if (player.wallsDetected)
        {
            player.sm.ChangeState(player.wallSlideState);    
        }

        if (player.groundDetected && player.rb.linearVelocityY < 0.0 && !touchedGround)
        {
            player.SetVelocityX(0.0f);
            touchedGround = true;

            player.animator.SetTrigger("launchAttackTrigger");
        }

        if (!touchedGround)
        {
            player.SetVelocityX(player.launchAttackForce.x * player.FacingDirScale());
        }

        
        if (stateEnded && player.groundDetected)
        {
            player.sm.ChangeState(player.idleState);
        }
    }
}
