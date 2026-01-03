using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState { get; protected set; }
    public Enemy_MoveState moveState { get; protected set; }

    [Header("Movement Details")]
    public float idleTime = 2.0f;
    public float moveSpeed = 1.4f;
    public float moveSpeedMultiplier = 1.0f;
}
