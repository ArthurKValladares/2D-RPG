using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Entity entity;
    private Entity_VFX vfxComponent;
    private Slider healthBar;
    [SerializeField] private GameObject healthBarObject;

    private Entity_Stats stats;

    [Header("Health Details")]
    [SerializeField] protected float currentHealth;

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

        SetHP(stats.CalculateMaxHP());
    }

    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (currentHealth <= 0.0f) return false;
        if (AttackEvaded())
        {
            // TODO: Evasion effect
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null
            ? attackerStats.GetArmorReduction()
            : 0.0f;

        float mitigation = stats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1.0f - mitigation);
        Debug.Log(finalDamage);

        ReduceHP(finalDamage);

        if (entity)
        {
            Vector2 force = GetKnockbackForce(finalDamage, damageDealer);
            float duration = GetKnockbackDuration(finalDamage);
            entity.ReceivePush(force, duration);
        }

        if (vfxComponent)
        {
            vfxComponent.PlayOnDamageVFX();
        }

        if (currentHealth <= 0.0f)
        {
            Die();
        }

        return true;
    }

    private bool IsHeavyAttack(float damage)
    {
        float damageHealthPercentage = damage / stats.CalculateMaxHP();
        return damageHealthPercentage > heavyKnockbackThreshold;
    }

    private Vector2 GetKnockbackForce(float damage, Transform damageDealer)
    {
        Vector2 knockback = IsHeavyAttack(damage)
            ? heavyKnockbackForce
            : knockbackForce;

        return IDamagable.GetKnockbackForceAwayFromDamage(knockback, transform, damageDealer);
    }

    private float GetKnockbackDuration(float damage)
    {
        return IsHeavyAttack(damage)
            ? heavyKnockbackDuration
            : knockbackDuration;
    }

    private bool AttackEvaded()
    {
        float evasion = stats.CalculateEvasion();
        return Random.Range(0.0f, 1.0f) < evasion; 
    }

    private void SetHP(float hp)
    {
        currentHealth = hp;
        UpdateHealthBar();
    }

    private void ReduceHP(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.value = (float) currentHealth / stats.CalculateMaxHP();
    }

    protected virtual void Die()
    {
        entity.EntityDeath();

        if (healthBarObject) {
            healthBarObject.SetActive(false);
        }
    }
}
