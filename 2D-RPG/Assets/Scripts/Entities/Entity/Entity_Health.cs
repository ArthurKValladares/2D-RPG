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

    private float CalculateFinalPhysicalDamage(float physicalDamage, Entity_Stats attackerStats)
    {
        float armorReduction = attackerStats != null
            ? attackerStats.GetArmorReduction()
            : 0.0f;

        float armorMitigation = stats.GetArmorMitigation(armorReduction);
        float finalPhysicalDamage = physicalDamage * (1.0f - armorMitigation);

        return finalPhysicalDamage;
    }

    private float CalculateFinalElementalDamage(ElementalDamageInfo elementalDamage)
    {
        float primaryElementalResistance = stats.GetElementalResistance(elementalDamage.primaryType);
        float primaryElementalDamage = elementalDamage.primaryDamage * (1.0f - primaryElementalResistance);

        float secondaryElementalResistance = stats.GetElementalResistance(elementalDamage.secondaryType);
        float secondaryElementalDamage = elementalDamage.secondaryDamage * (1.0f - secondaryElementalResistance);

        float finalElementalDamage = primaryElementalDamage + secondaryElementalDamage;

        return finalElementalDamage;
    }

    public virtual bool TakeDamage(float physicalDamage, ElementalDamageInfo elementalDamage, Transform damageDealer)
    {
        if (currentHealth <= 0.0f) return false;
        if (AttackEvaded())
        {
            // TODO: Evasion effect
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();

        float finalPhysicalDamage = CalculateFinalPhysicalDamage(physicalDamage, attackerStats);
        float finalElementalDamage = CalculateFinalElementalDamage(elementalDamage);

        float finalDamage = finalPhysicalDamage + finalElementalDamage;
        Debug.Log("Physical Damage: " + finalPhysicalDamage + " Elemental Damage: " + finalElementalDamage);

        if (entity)
        {
            Vector2 force = GetKnockbackForce(finalPhysicalDamage, damageDealer);
            float duration = GetKnockbackDuration(finalPhysicalDamage);
            entity.ReceivePush(force, duration);
        }

        if (vfxComponent)
        {
            vfxComponent.PlayOnDamageVFX();
        }

        ReduceHP(finalDamage);

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

    public void ReduceHP(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0.0f)
        {
            Die();
        }
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
