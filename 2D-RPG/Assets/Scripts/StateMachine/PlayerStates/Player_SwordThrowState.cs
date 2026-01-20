using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    public Player_SwordThrowState(Player player)
        : base(player, "swordThrow")
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skillManager.swordThrow.EnableDots(true);
    }

    public override void Exit()
    {
        base.Exit();

        SetSwordThrowPerformed(false);
        player.skillManager.swordThrow.EnableDots(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityX(0);

        Vector2 dirToMouse = player.DirectionToMouse();
        player.HandleFlip(dirToMouse.x);
        player.skillManager.swordThrow.PredictTrajectory(dirToMouse);

        if (player.input.Player.Attack.WasPressedThisFrame())
        {
            player.skillManager.swordThrow.ConfirmTrajectory(dirToMouse);
            player.skillManager.swordThrow.EnableDots(false);

            SetSwordThrowPerformed(true);
        }

        if (player.input.Player.RangedAttack.WasReleasedThisFrame() || stateEnded)
        {
            player.sm.ChangeState(player.idleState);
        }
    }

    private void SetSwordThrowPerformed(bool performed)
    {
        player.animator.SetBool("swordThrowPerformed", performed);
    }
}
