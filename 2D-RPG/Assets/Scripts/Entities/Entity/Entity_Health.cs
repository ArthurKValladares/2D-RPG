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
    [SerializeField] protected float currentHP;
    [SerializeField] private bool canRegenerateHealth = true;
    [SerializeField] private float healthRegenInterval = 0.5f;

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
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.CompareTag("HealthBar"))
            {
                healthBar = slider;
                break;
            }
        }
        stats = GetComponentInChildren<Entity_Stats>();

        SetHP(stats.CalculateMaxHP());

        InvokeRepeating(nameof(RegenerateHealth), 0.0f, healthRegenInterval);
    }

    private void RegenerateHealth()
    {
        float healthRegenPerSecond = stats.resources.healthRegenPerSecond.GetValue();
        if (!canRegenerateHealth || healthRegenPerSecond <= 0.0f) return;

        float ticksPerSecond = 1.0f / healthRegenInterval;
        float healthRegenPerTick = healthRegenPerSecond / ticksPerSecond;

        IncreaseHP(healthRegenPerTick);
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

    public virtual HitInfo TakeDamage(PhysicalDamageInfo physicalDamage, ElementalDamageInfo elementalDamage, Transform damageDealer)
    {
        if (currentHP <= 0.0f) return new HitInfo(false);
        if (AttackEvaded())
        {
            // TODO: Evasion effect
            return new HitInfo(false);
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();

        float finalPhysicalDamage = CalculateFinalPhysicalDamage(physicalDamage.damageResult, attackerStats);
        float finalElementalDamage = CalculateFinalElementalDamage(elementalDamage);

        float finalDamage = finalPhysicalDamage + finalElementalDamage;

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

        bool killedVictim = currentHP <= 0.0f;
        return new HitInfo(true, finalDamage, killedVictim);
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

    public float GetCurrentHPPercentage()
    {
        return currentHP / stats.CalculateMaxHP();
    }

    public void SetHP(float hp)
    {
        currentHP = hp;
        UpdateHealthBar();
    }

    public void SetHPPercentage(float percentage)
    {
        SetHP(Mathf.Clamp01(percentage) * stats.CalculateMaxHP());
    }

    public void ReduceHP(float damage)
    {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0.0f)
        {
            Die();
        }
    }

    public void IncreaseHP(float amount)
    {
        if (currentHP <= 0.0) return;

        float newHealth = Mathf.Min(currentHP + amount, stats.CalculateMaxHP());

        SetHP(newHealth);
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        healthBar.value = (float) currentHP / stats.CalculateMaxHP();
    }

    protected virtual void Die()
    {
        entity.EntityDeath();

        if (healthBarObject) {
            healthBarObject.SetActive(false);
        }
    }
}
