using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    private Skill_TimeEcho timeEcho;

    [Space]
    [SerializeField] private GameObject onDeathVFX;

    [Space]
    [SerializeField] private float damageRadius = 1.0f;
    public int maxAttacks;

    private TrailRenderer wispTrail;
    private Transform playerTranform;
    private float wispMoveSpeed;
    private bool isWisp = false;

    [Header("Collision Detection")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatIsGround;

    public void SetupTimeEcho(Skill_TimeEcho timeEcho)
    {
        this.timeEcho = timeEcho;
        this.maxAttacks = timeEcho.GetMaxAttacks();

        this.wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);
        this.playerTransform = timeEcho.transform.root;
        this.wispMoveSpeed = timeEcho.wispMoveSpeed;

        SetupSkillData(timeEcho);

        anim.SetBool("canAttack", maxAttacks > 0);

        FlipToTarget();

        Invoke(nameof(HandleDeath), timeEcho.GetEchoDuration());
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, damageRadius);

        if (targetGotHit)
        {
            bool shoudlDuplicate = Random.value < timeEcho.GetDuplicateChance();

            if (shoudlDuplicate)
            {
                float xOffset = (transform.position.x < lastTarget.position.x) ? 1.0f : -1.0f;

                timeEcho.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0, 0));
            }
        }
    }

    private void Update()
    {
        if (isWisp)
        {
            HandleWispMovement();
        } else
        {
            anim.SetFloat("yVelocity", rb.linearVelocityY);
            StopHorizontalMovement();
        }
    }

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();
        if (target == null) return;

        if (target.position.x < transform.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
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

        if (timeEcho.ShouldBeWisp())
        {
            TurnIntoWisp();
        } else
        {
            Destroy(gameObject);
        }
    }

    private void TurnIntoWisp()
    {
        isWisp = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, wispMoveSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        // TODO: Sorta repeat this in sword throw, maybe standardize somehow
        float distanceEpsilon = 0.5f;
        if (distance < distanceEpsilon)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
