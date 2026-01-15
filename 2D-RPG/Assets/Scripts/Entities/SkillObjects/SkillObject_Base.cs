using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float targetCheckRadius = 1.0f;

    private void Awake()
    {
        if (targetCheck == null)
        {
            targetCheck = transform;
        }
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        Collider2D[] enemies = EnemiesAround(t, radius);
        foreach (Collider2D enemyCollider in enemies)
        {
            IDamagable damagable = enemyCollider.GetComponent<IDamagable>();
            if (damagable == null) continue;

            // TODO: hard-coded for now
            PhysicalDamageInfo physicalDamageInfo = new PhysicalDamageInfo(1.0f, false);
            ElementalDamageInfo elementalInfo = new ElementalDamageInfo(1.0f, ElementalDamageType.None);
            HitInfo hitInfo = damagable.TakeDamage(physicalDamageInfo, elementalInfo, transform);
        }
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
