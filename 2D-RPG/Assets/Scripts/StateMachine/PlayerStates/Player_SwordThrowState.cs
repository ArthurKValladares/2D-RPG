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

        SetSwordThrowPerformed(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityX(0);

        Vector2 dirToMouse = player.DirectionToMouse();
        player.HandleFlip(dirToMouse.x);

        if (player.input.Player.RangedAttack.WasReleasedThisFrame() || stateEnded)
        {
            player.sm.ChangeState(player.idleState);
            return;
        }

        if (player.input.Player.Attack.WasPressedThisFrame())
        {
            SetSwordThrowPerformed(true);
        }
    }

    private void SetSwordThrowPerformed(bool performed)
    {
        player.animator.SetBool("swordThrowPerformed", performed);
    }
}
