using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private Entity entity;
    private SpriteRenderer sr;
    private Color originalColor;
    private Material originalMaterial;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Coroutine onDamageCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color onHitTargetColor = Color.white;
    [SerializeField] private GameObject onHitTargetVFX;    
    [SerializeField] private GameObject onCritHitTargetVFX;

    [Header("Element VFX")]
    [SerializeField] private Color fireColor = Color.red;
    [SerializeField] private Color iceColor = Color.cyan;
    [SerializeField] private Color lightningColor = Color.yellow;
    [SerializeField] private float blinkingColorTicknterval = 0.4f;
    private Coroutine onStatusVFXCoroutine;

    private void Awake()
    {
        entity = GetComponentInChildren<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
        originalMaterial = sr.material;
    }

    private Color GetOnHitVFXColor(ElementalDamageType ty)
    {
        switch (ty)
        {
            case ElementalDamageType.None:
                return onHitTargetColor;
            case ElementalDamageType.Fire:
                return fireColor;
            case ElementalDamageType.Ice:
                return iceColor;
            case ElementalDamageType.Lightning: 
                return lightningColor;
        }

        return onHitTargetColor;
    }

    public void CreateOnHitTargetVFX(Transform target, bool wasCritical, ElementalDamageType elementalTy)
    {
        GameObject hitPrefab = wasCritical ? onCritHitTargetVFX : onHitTargetVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        SpriteRenderer vfxSR = vfx.GetComponentInChildren<SpriteRenderer>();
        vfxSR.color = GetOnHitVFXColor(elementalTy);

        if (wasCritical && entity.FacingDirScale() == -1.0f)
        {
            vfx.transform.Rotate(0, 180, 0);
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageCoroutine != null)
        {
            StopCoroutine(onDamageCoroutine);
        }

        onDamageCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    private IEnumerator OnDamageVFXCoroutine()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVFXDuration);

        sr.material = originalMaterial;
    }

    public void PlayOnStatusVFX(float duration, ElementalDamageType element)
    {
        if (onStatusVFXCoroutine != null)
        {
            StopCoroutine(onStatusVFXCoroutine);
        }

        onStatusVFXCoroutine = StartCoroutine(PlayOnStatusVFXCoroutine(duration, element));
    }

    private IEnumerator PlayOnStatusVFXCoroutine(float duration, ElementalDamageType element)
    {
        Color color = GetOnHitVFXColor(element);
        return PlayBlinkingColorCoroutine(duration, color);
    }

    private IEnumerator PlayBlinkingColorCoroutine(float duration, Color color)
    {
        // TODO: Could specify this by hand later if I want
        Color lighterColor = color * 1.2f;
        Color darkerColor = color * 0.8f;

        float timer = 0.0f;
        bool toggle = false;

        while (timer < duration)
        {
            sr.color = toggle ? lighterColor : darkerColor;
            toggle = !toggle;

            yield return new WaitForSeconds(blinkingColorTicknterval);

            timer += blinkingColorTicknterval;
        }

        sr.color = originalColor;
    }
}
