using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this);
        moveState = new Enemy_MoveState(this);
        attackState = new Enemy_AttackState(this);
        battleState = new Enemy_BattleState(this);

        attackDistance = 2.0f;
    }

    protected override void Start()
    {
        base.Start();

        sm.Initialize(idleState);
    }
}
