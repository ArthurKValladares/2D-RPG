using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState { get; protected set; }
    public Enemy_MoveState moveState { get; protected set; }
    public Enemy_BattleState battleState { get; protected set; }
    public Enemy_AttackState attackState { get; protected set; }

    [Header("Movement Details")]
    public float idleTime = 2.0f;
    public float moveSpeed = 1.4f;
    public float moveSpeedMultiplier = 1.0f;

    [Header("Player Detection")]
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected LayerMask whatIsPlayer;

    protected override void Awake()
    {
        base.Awake();

        whatIsPlayer = LayerMask.GetMask("Player");
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * FacingDirScale(), playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != whatIsPlayer)
        {
            return default;
        }

        return hit;

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(playerCheckDistance * FacingDirScale(), 0, 0));
    }
}
