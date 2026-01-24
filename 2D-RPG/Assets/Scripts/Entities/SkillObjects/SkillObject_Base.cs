using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [Header("Target Checking")]
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float targetCheckRadius = 1.0f;
    [Header("Surround Checking")]
    [SerializeField] protected float surroundCheckRadius = 10.0f;

    protected Entity_Stats playerStats;
    protected Entity_VFX playerVFX;
    protected Transform playerTransform;

    protected Animator anim;

    protected DamageScaleData damageScaleData;
    protected ElementalDamageType primaryElementalDamage;
    protected ElementalDamageType secondaryElementalDamage;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        Collider2D[] enemies = EnemiesAround(t, radius);
        foreach (Collider2D target in enemies)
        {
            if (!target.TryGetComponent<IDamagable>(out var damagable)) continue;

            AttackData attackData = new AttackData(playerStats, damageScaleData, primaryElementalDamage, secondaryElementalDamage);

            HitInfo hitInfo = damagable.TakeDamage(attackData, transform);
            // TODO: THis name is not really true, it does more than that
            attackData.ApplyElementalEffect(playerVFX, target, hitInfo);
        }
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in EnemiesAround(transform, surroundCheckRadius))
        {
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }

        return target;
    }

    protected void SetupSkillData(Skill_Base skill)
    { 
        damageScaleData = skill.damageScaleData;
        primaryElementalDamage = skill.primaryElementalDamage;
        secondaryElementalDamage = skill.secondaryElementalDamage;

        playerStats = skill.player.stats;
        playerTransform = skill.player.transform.root;
        playerVFX = skill.player.playerVFX;
    }

    protected virtual void OnDrawGizmos()
    {
        // TODO: is this the right place for this assingnment?
        if (targetCheck == null)
        {
            targetCheck = transform;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetCheck.position, surroundCheckRadius);
    }
}
