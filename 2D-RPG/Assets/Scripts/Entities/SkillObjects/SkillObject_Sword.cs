using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordThrow;
    protected Rigidbody2D rb;

    [SerializeField] private float comebackSpeed = 20.0f;
    [SerializeField] private float maxDistanceFromPlayer = 25.0f;
    protected bool shouldComeback;

    public virtual void SetupSword(Skill_SwordThrow swordThrow, Vector2 throwForce)
    {
        this.swordThrow = swordThrow;

        SetupSkillData(swordThrow);

        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = throwForce;
    }

    public void SendSwordBackToPlayer()
    {
        shouldComeback = true;
    }

    private void Update()
    {
        transform.right = rb.linearVelocity;

        HandleComeback();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform, 1.0f);
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
}
