using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordThrow;

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

        // TODO: Sorta repeat this in wisp/time echo, maybe standardize somehow
        float distanceEpsilon = 0.5f;
        if (distance < distanceEpsilon)
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
