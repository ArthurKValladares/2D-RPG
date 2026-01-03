using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Windows;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; protected set; }
    public Animator animator { get; protected set; }

    public StateMachine sm { get; protected set; }
    
    protected bool facingRight = true;

    [Header("Entity Collision Detection")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform primaryWallCheck;
    [SerializeField] protected Transform secondaryWallCheck;
    [SerializeField] protected LayerMask whatIsGround;
    [field: SerializeField] public bool groundDetected { get; protected set; }

    [field: SerializeField] public bool primaryWallDetected { get; protected set; }
    [field: SerializeField] public bool secondaryWallDetected { get; protected set; }
    [field: SerializeField] public bool wallsDetected { get; protected set; }


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sm = new StateMachine();

        whatIsGround = LayerMask.GetMask("Ground");
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();

        sm.UpdateActiveState();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));

        Vector3 wallCheckVector = new(FacingDirScale() * wallCheckDistance, 0, 0);
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + wallCheckVector);
        if (secondaryWallCheck)
        {
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + wallCheckVector);
        }
    }

    public void SetCurrentStateEnded()
    {
        sm.currentState.StateEnded();
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        Vector2 wallCheckVector = Vector2.right * FacingDirScale();
        primaryWallDetected = Physics2D.Raycast(primaryWallCheck.position, wallCheckVector, wallCheckDistance, whatIsGround);
        wallsDetected = primaryWallDetected;
        if (secondaryWallCheck)
        {
            secondaryWallDetected = Physics2D.Raycast(secondaryWallCheck.position, wallCheckVector, wallCheckDistance, whatIsGround);
            wallsDetected = wallsDetected && secondaryWallDetected;
        }
    }

    public float FacingDirScale()
    {
        return facingRight ? 1.0f : -1.0f;
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
}
