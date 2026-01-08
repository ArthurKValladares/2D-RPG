using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Entity entity;
    private Entity_VFX vfxComponent;
    private Slider healthBar;

    [SerializeField] private Entity_Stats stats;

    [Header("Health Details")]
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
        stats = GetComponentInChildren<Entity_Stats>();

        currentHealth = stats.CalculateMaxHP();
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
            if (((float)damage / stats.CalculateMaxHP()) >= heavyKnockbackThreshold)
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

        healthBar.value = (float) currentHealth / stats.CalculateMaxHP();
    }

    protected void Die()
    {
        entity.EntityDeath();
    }
}
