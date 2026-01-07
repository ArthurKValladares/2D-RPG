using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamagable
{
    Entity entity;
    Entity_VFX vfxComponent;

    [Header("Health Details")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    [Header("On-hit Knockback Details")]
    [SerializeField] private float heavyKnockbackThreshold;
    [SerializeField] private Vector2 knockbackForce;
    [SerializeField] private Vector2 heavyKnockbackForce;
    [SerializeField] private float knockbackDuration;    
    [SerializeField] private float heavyKnockbackDuration;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        entity = GetComponent<Entity>();
        vfxComponent = GetComponent<Entity_VFX>();
    }

    public virtual void TakeDamage(int damage, Transform damageDealer)
    {
        if (currentHealth < 0) return;

        currentHealth -= damage;

        if (entity)
        {
            Vector2 knockbackToApply = knockbackForce;
            if (((float)damage / maxHealth) >= heavyKnockbackThreshold)
            {
                knockbackToApply = heavyKnockbackForce;
            }

            entity.ReceivePush(IDamagable.GetKnockbackForceAwayFromDamage(knockbackToApply, transform, damageDealer), knockbackDuration);
        }

        if (vfxComponent)
        {
            vfxComponent.PlayOnDamageVFX();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        entity.EntityDeath();
    }
}
