using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    Entity_VFX vfxComponent;

    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        vfxComponent = GetComponent<Entity_VFX>();
    }

    public virtual void TakeDamage(int damage, Transform damageDealer)
    {
        currentHealth -= damage;

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
        Debug.Log("Entity died");
    }
}
