using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Entity entity;
    private Entity_VFX vfxComponent;
    private Slider healthBar;

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
        entity = GetComponent<Entity>();
        vfxComponent = GetComponent<Entity_VFX>();
        healthBar = GetComponentInChildren<Slider>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public virtual void TakeDamage(int damage, Transform damageDealer)
    {
        if (currentHealth < 0) return;

        currentHealth -= damage;
        UpdateHealthBar();

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

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.value = (float) currentHealth / maxHealth;
    }

    protected void Die()
    {
        entity.EntityDeath();
    }
}
