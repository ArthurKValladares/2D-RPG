using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this);
        moveState = new Enemy_MoveState(this);
    }

    protected override void Start()
    {
        base.Start();

        sm.Initialize(idleState);
    }
}
