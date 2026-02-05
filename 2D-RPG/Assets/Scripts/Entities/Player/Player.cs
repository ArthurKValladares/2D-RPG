using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    // TODO: In the future, this and the input set for it should not be in Player
    private UI ui;

    private CapsuleCollider2D capsuleCollider;

    public static event Action OnPlayerDeath;

    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX playerVFX;

    public PlayerInputSet input { get; private set; }
    public Entity_Health health;
    public Entity_StatusHandler statusHandler { get; private set; }

    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }

    #region StateVariables
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
    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }
    #endregion

    public float originalGravityscale { get; private set; }

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float jumpForce = 12.0f;
    [Range(0.0f, 1.0f)] public float inAirMoveMultiplier = 0.8f;
    [Range(0.0f, 1.0f)] public float wallSlideMultiplier = 0.4f;
    [SerializeField] private Vector2 wallJumpForce = new(6.0f, 12.0f);
    public float wallJumpNoMovementTimer = 0.1f;
    public float dashForce = 20.0f;
    [Space] public float dashTime = 0.25f;

    [Header("Attack Details")]
    public const int NumBasicAttacks = 3;
    [SerializeField] private Vector2[] attackVelocities = new Vector2[NumBasicAttacks];
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.3f;
    private Coroutine queuedAttackCoroutine;

    protected override void Awake()
    {
        base.Awake();

        ui = FindFirstObjectByType<UI>();

        capsuleCollider = GetComponent<CapsuleCollider2D>();
        skillManager = GetComponent<Player_SkillManager>();
        playerVFX = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();

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
        swordThrowState = new Player_SwordThrowState(this);
        domainExpansionState = new Player_DomainExpansionState(this);

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

        input.Player.Mouse.performed += ctx =>
        {
            mousePosition = ctx.ReadValue<Vector2>();
        };

        input.Player.ToggleSkillTree.performed += ctx => {
            ui.ToggleSkillTree();
        };

        // TODO: better system for different skills later
        input.Player.Spell.performed += ctx =>
        {
            skillManager.shard.TryToUseSkill();
        };
        input.Player.Spell.performed += ctx =>
        {
            skillManager.timeEcho.TryToUseSkill();
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

    public void TeleportPlayer(Vector2 pos)
    {
        transform.position = pos;
    }

    public Vector2 DirectionToMouse()
    {
        Vector2 playerPos = transform.position;
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 dir = worldMousePos - playerPos;

        return dir.normalized;
    }

    public float OpenDistanceAbovePlayer(float maxDistance = float.PositiveInfinity)
    {
        float yEpsilon = 0.5f;
        float distanceToTopOfCollider = (capsuleCollider.size.y / 2.0f) + capsuleCollider.offset.y + yEpsilon;

        float effectiveMaxDistance = maxDistance == float.PositiveInfinity 
            ? float.PositiveInfinity 
            : maxDistance - distanceToTopOfCollider;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, effectiveMaxDistance, whatIsGround);

        return hit.collider != null ? (hit.distance - distanceToTopOfCollider) : effectiveMaxDistance;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed * activeSlowMultiplier;
    }

    public float GetJumpForce()
    {
        return jumpForce * activeSlowMultiplier;
    }

    public Vector2 GetWallJumpForce()
    {
        return wallJumpForce * activeSlowMultiplier;
    }

    public Vector2 GetAttackVelocityAt(int idx)
    {
        return attackVelocities[idx] * activeSlowMultiplier;
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        sm.ChangeState(deadState);
        OnPlayerDeath?.Invoke();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (capsuleCollider)
        {
            float distance = OpenDistanceAbovePlayer(30.0f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, distance, 0));
        }
    }
}
