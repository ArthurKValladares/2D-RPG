using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    [SerializeField] private float maxDistance;
    [SerializeField] private float attacksPerSecond;
    private float attackTimer;

    public override void SetupSword(Skill_SwordThrow swordThrow, Vector2 throwForce)
    {
        base.SetupSword(swordThrow, throwForce);

        if (anim)
        {
            anim.SetTrigger("spin");
        }

        maxDistance = swordThrow.maxTravelDistance;
        attacksPerSecond = swordThrow.attacksPerSecond;
        attackTimer = 0.0f;

        Invoke(nameof(SendSwordBackToPlayer), swordThrow.maxSpinDuration);
    }

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();   
    }

    private void HandleStopping()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceFromPlayer > maxDistance && rb.simulated)
        {
            rb.simulated = false;
        }
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0.0f)
        {
            DamageEnemies();
            attackTimer = 1.0f / attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
