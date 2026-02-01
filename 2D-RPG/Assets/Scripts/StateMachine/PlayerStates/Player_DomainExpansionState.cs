using UnityEngine;

public class Player_DomainExpansionState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;

    private float finalRiseDistance;

    private bool isLevitating;
    private bool createdDomain;

    // TODO: These need to be a part of the skill later
    static float riseMaxDistance = 3.0f;
    static float riseSpeed = 25.0f;

    public Player_DomainExpansionState(Player player)
        : base(player, "jumpFall")
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = player.rb.gravityScale;

        finalRiseDistance = player.OpenDistanceAbovePlayer(riseMaxDistance);

        player.SetVelocity(0.0f, riseSpeed);
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = originalGravity;

        isLevitating = false;
        createdDomain = false;
    }

    public override void Update()
    {
        base.Update();

        float currDistance = Vector2.Distance(originalPosition, player.transform.position);
        if (currDistance >= finalRiseDistance && isLevitating == false)
        {
            Levitate();
        }

        if (isLevitating)
        {
            player.skillManager.domainExpansion.DoSpellCasting();

            if (TimerDone())
            {
                player.sm.ChangeState(player.idleState);
            }
        }
    }

    private void Levitate()
    {
        isLevitating = true;
        player.rb.linearVelocity = Vector2.zero;
        player.rb.gravityScale = 0.0f;

        stateTimer = player.skillManager.domainExpansion.GetDomainDuration();

        if (!createdDomain)
        {
            createdDomain = true;
            player.skillManager.domainExpansion.CreateDomain();
        }
    }
}
