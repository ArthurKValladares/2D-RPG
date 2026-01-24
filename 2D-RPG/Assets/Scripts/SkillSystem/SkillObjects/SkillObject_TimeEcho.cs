using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    private Skill_TimeEcho timeEcho;

    [Space]
    [SerializeField] private GameObject onDeathVFX;

    [Header("Collision Detection")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsGround;

    public void SetupTimeEcho(Skill_TimeEcho timeEcho)
    {
        this.timeEcho = timeEcho;

        Invoke(nameof(HandleDeath), timeEcho.GetEchoDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocityY);

        StopHorizontalMovement();
    }

    private void StopHorizontalMovement()
    {
        bool groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (groundDetected)
        {
            rb.linearVelocityX = 0.0f;
        }
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
    }
}
