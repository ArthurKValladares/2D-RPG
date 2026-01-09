using UnityEngine;

public class ElementalDamageInfo
{
    public ElementalDamageInfo(
        float primaryDamage, ElementalDamageType primaryType,
        float secondaryDamage = 0.0f, ElementalDamageType secondaryType = ElementalDamageType.None)
    {
        this.primaryDamage = primaryDamage;
        this.primaryType = primaryType;

        this.secondaryDamage = secondaryDamage;
        this.secondaryType = secondaryType;
    } 

    public float primaryDamage;
    public ElementalDamageType primaryType;
    public float secondaryDamage;
    public ElementalDamageType secondaryType;
};

public interface IDamagable
{
    public static Vector2 GetKnockbackForceAwayFromDamage(Vector2 knockback, Transform entityTransform, Transform damageDealer)
    {
        float knockbackDirScale = (entityTransform.position.x > damageDealer.position.x)
                ? 1.0f
                : -1.0f;

        return new Vector2(knockback.x * knockbackDirScale, knockback.y);
    }

    public bool TakeDamage(float physicalDamage, ElementalDamageInfo elementalDamage, Transform damageDealer);
}

