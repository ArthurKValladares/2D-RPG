using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordThrow;
    protected Rigidbody2D rb;

    [Header("Sword Return Details")]
    private float comebackSpeed;
    protected bool shouldComeback;
    [SerializeField] private float maxDistanceFromPlayer = 25.0f;

    [Header("Damage Details")]
    [SerializeField] protected float damageRadius = 1.0f;

    public virtual void SetupSword(Skill_SwordThrow swordThrow, Vector2 throwForce)
    {
        this.swordThrow = swordThrow;
        this.comebackSpeed = swordThrow.currentComebackSpeed;

        SetupSkillData(swordThrow);

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = throwForce;
    }

    public void SendSwordBackToPlayer()
    {
        shouldComeback = true;
    }

    protected virtual void Update()
    {
        transform.right = rb.linearVelocity;

        HandleComeback();
    }

    protected void DamageEnemies()
    {
        DamageEnemiesInRadius(transform, damageRadius);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemies();    
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistanceFromPlayer)
        {
            SendSwordBackToPlayer();
        }

        if (!shouldComeback) return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed * Time.deltaTime);

        if (distance < 0.5f)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
