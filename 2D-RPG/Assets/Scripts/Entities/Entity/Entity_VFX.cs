using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private Entity entity;
    private SpriteRenderer sr;
    private Material originalMaterial;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Coroutine onDamageCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject onHitTargetVFX;
    [SerializeField] private Color onHitTargetColor = Color.white;
    [SerializeField] private GameObject onCritHitTargetVFX;

    private void Awake()
    {
        entity = GetComponentInChildren<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitTargetVFX(Transform target, bool wasCritical)
    {
        GameObject hitPrefab = wasCritical ? onCritHitTargetVFX : onHitTargetVFX;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        SpriteRenderer vfxSR = vfx.GetComponentInChildren<SpriteRenderer>();
        vfxSR.color = onHitTargetColor;

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
}
