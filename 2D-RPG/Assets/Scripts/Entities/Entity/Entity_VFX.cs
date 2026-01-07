using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Material originalMaterial;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.15f;
    private Coroutine onDamageCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject onHitTargetVFX;
    [SerializeField] private Color onHitTargetColor = Color.white;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitTargetVFX(Transform target)
    {
        GameObject vfx = Instantiate(onHitTargetVFX, target.position, Quaternion.identity);
        SpriteRenderer vfxSR = vfx.GetComponentInChildren<SpriteRenderer>();
        vfxSR.color = onHitTargetColor;
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
