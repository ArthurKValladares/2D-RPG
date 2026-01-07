using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    
    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }

    public Player_IdleState idleState { get; protected set; }
    public Player_MoveState moveState { get; protected set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_LaunchAttackState launchAttackState { get; private set; }
    public Player_HurtState hurtState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_ParryState parryState { get; private set; }

    public float originalGravityscale { get; private set; }

    [Header("Movement Details")]
    public float moveSpeed = 8.0f;
    public float jumpForce = 12.0f;
    [Range(0.0f, 1.0f)] public float inAirMoveMultiplier = 0.8f;
    [Range(0.0f, 1.0f)] public float wallSlideMultiplier = 0.4f;
    public Vector2 wallJumpForce = new(6.0f, 12.0f);
    public float wallJumpNoMovementTimer = 0.1f;
    public float dashForce = 20.0f;
    [Space] public float dashTime = 0.25f;

    [Header("Attack Details")]
    public const int NumBasicAttacks = 3;
    public Vector2[] attackVelocities = new Vector2[NumBasicAttacks];
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.3f;
    private Coroutine queuedAttackCoroutine;
    public Vector2 launchAttackForce = new(8.0f, 15.0f);
    public float launchAttackDuration = 3.0f;

    protected override void Awake()
    {
        base.Awake();

        originalGravityscale = rb.gravityScale;

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this);
        moveState = new Player_MoveState(this);
        jumpState = new Player_JumpState(this);
        fallState = new Player_FallState(this);
        wallSlideState = new Player_WallSlideState(this);
        wallJumpState = new Player_WallJumpState(this);
        dashState = new Player_DashState(this);
        basicAttackState = new Player_BasicAttackState(this);
        jumpAttackState = new Player_JumpAttackState(this);
        launchAttackState = new Player_LaunchAttackState(this);
        hurtState = new Player_HurtState(this);
        deadState = new Player_DeadState(this);
        parryState = new Player_ParryState(this);

        attackVelocities[0] = new Vector2(3.0f, 1.5f);
        attackVelocities[1] = new Vector2(1.5f, 1.5f);
        attackVelocities[2] = new Vector2(4.0f, 5.0f);
    }

    protected override void Start()
    {
        base.Start();

        sm.Initialize(idleState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();
        };
        input.Player.Movement.canceled += ctx => {
            moveInput = Vector2.zero;
        };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null) {
            StopCoroutine(queuedAttackCoroutine);
        }

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();

        sm.ChangeState(basicAttackState);
    }

    public void TryEnteringHurtState()
    {
        if (sm.currentState == hurtState) return;

        sm.ChangeState(hurtState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        sm.ChangeState(deadState);
        OnPlayerDeath?.Invoke();
    }
}
