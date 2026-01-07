using UnityEngine;

public interface IDamagable
{
    public static Vector2 GetKnockbackForceAwayFromDamage(Vector2 knockback, Transform entityTransform, Transform damageDealer)
    {
        float knockbackDirScale = (entityTransform.position.x > damageDealer.position.x)
                ? 1.0f
                : -1.0f;

        return new Vector2(knockback.x * knockbackDirScale, knockback.y);
    }

    public void TakeDamage(int damage, Transform damageDealer);
}

