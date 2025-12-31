using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput { get; private set; }

    public StateMachine sm { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public float originalGravityscale { get; private set; }

    public Animator animator { get; private set; }

    [Header("Movement Details")]
    public float moveSpeed = 8.0f;
    public float jumpForce = 12.0f;
    [Range(0.0f, 1.0f)] public float inAirMoveMultiplier = 0.8f;
    [Range(0.0f, 1.0f)] public float wallSlideMultiplier = 0.4f;
    public Vector2 wallJumpForce = new Vector2(6.0f, 12.0f);
    public float dashForce = 20.0f;
    [Space] public float dashTime = 0.25f;

    private bool facingRight = true;

    [Header("Collision Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckEpsilon = 0.01f;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallCheckEpsilon = 0.01f;
    [SerializeField] private LayerMask whatIsGround;
    [field: SerializeField] public bool groundDetected { get; private set; }
    [field: SerializeField] public bool wallDetected { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityscale = rb.gravityScale;
        animator = GetComponentInChildren<Animator>();

        input = new PlayerInputSet();

        sm = new StateMachine();
        idleState = new Player_IdleState(this);
        moveState = new Player_MoveState(this);
        jumpState = new Player_JumpState(this);
        fallState = new Player_FallState(this);
        wallSlideState = new Player_WallSlideState(this);
        wallJumpState = new Player_WallJumpState(this);
        dashState = new Player_DashState(this);

        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        groundCheckDistance = capsuleCollider.size.y / 2.0f - capsuleCollider.offset.y + groundCheckEpsilon;
        wallCheckDistance = capsuleCollider.size.x / 2.0f - capsuleCollider.offset.x + wallCheckEpsilon;

        whatIsGround = LayerMask.GetMask("Ground");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();

            HandleFlip(moveInput.x);
        };
        input.Player.Movement.canceled += ctx => {
            moveInput = Vector2.zero;
        };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        sm.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollisionDetection();

        sm.UpdateActiveState();
    }

    public void SetVelocityX(float xVel)
    {
        rb.linearVelocityX = xVel;
        HandleFlip(rb.linearVelocityX);
    }
    public void SetVelocityY(float yVel)
    {
        rb.linearVelocityY = yVel;
    }
    public void SetVelocity(float xVel, float yVel)
    {
        SetVelocityX(xVel);
        SetVelocityY(yVel);
    }

    private void HandleFlip(float xVel)
    {
        if (xVel > 0 && facingRight == false)
        {
            Flip();
        }
        else if (xVel < 0 && facingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * FacingDirScale(), wallCheckDistance, whatIsGround);
    }

    public float FacingDirScale()
    {
        return facingRight ? 1.0f : -1.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance, 0));

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(FacingDirScale() * wallCheckDistance, 0, 0));
    }
}
