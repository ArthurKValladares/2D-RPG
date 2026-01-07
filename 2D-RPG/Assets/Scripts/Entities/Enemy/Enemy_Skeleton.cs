using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this);
        moveState = new Enemy_MoveState(this);
        attackState = new Enemy_AttackState(this);
        battleState = new Enemy_BattleState(this);
        hurtState = new Enemy_HurtState(this);
        deadState = new Enemy_DeadState(this);
        stunnedState = new Enemy_StunnedState(this);

        attackDistance = 2.0f;
    }

    protected override void Start()
    {
        base.Start();

        sm.Initialize(idleState);
    }

    public bool HandleCounter()
    {
        // NOTE: changing the state could change canBeStunned, so we need to cache its start value
        bool canBeStunnedAtStart = canBeStunned;

        if (canBeStunnedAtStart)
        {
            sm.ChangeState(stunnedState);
        }

        return canBeStunnedAtStart;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.G))
        {
            HandleCounter();
        }
    }
}
