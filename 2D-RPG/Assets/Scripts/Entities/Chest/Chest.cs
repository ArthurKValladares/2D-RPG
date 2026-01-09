using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Chest : MonoBehaviour, IDamagable
{
    private Animator anim;
    private Rigidbody2D rb;
    Entity_VFX vfxComponent;

    [Header("Open Details")]
    [SerializeField] private Vector2 onOpenKnockback;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        vfxComponent = GetComponentInChildren<Entity_VFX>();
    }

    public bool TakeDamage(float _damage, Transform damageDealer)
    {
        if (anim.GetBool("open")) return false;

        anim.SetBool("open", true);

        rb.linearVelocity = IDamagable.GetKnockbackForceAwayFromDamage(onOpenKnockback, rb.transform, damageDealer);

        if (vfxComponent)
        {
            vfxComponent.PlayOnDamageVFX();
        }

        return true;
    }
}
