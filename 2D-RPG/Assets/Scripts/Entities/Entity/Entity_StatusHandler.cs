using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;

    private ElementalDamageType currentElement = ElementalDamageType.None;

    private Coroutine chillCoroutine;
    private Coroutine burnCoroutine;
    private Coroutine electrifyCoroutine;


    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
    }

    public bool CanApply(ElementalDamageType _element)
    {
        return currentElement == ElementalDamageType.None;
    }

    private float GetValueAfterResistance(float val, ElementalDamageType element)
    {
        float resistance = entityStats.GetElementalResistance(element);
        return val * (1.0f - resistance);
    }

    public void ApplyChillEffect(float duration, float slowPercentage)
    {
        float reducedDuration = GetValueAfterResistance(duration, ElementalDamageType.Ice);

        entity.SlowDownEntityBy(reducedDuration, slowPercentage);

        if (chillCoroutine != null)
        {
            StopCoroutine(chillCoroutine);
        }
        chillCoroutine = StartCoroutine(ApplyEffectVFXCoroutine(reducedDuration, ElementalDamageType.Ice));
    }

    public void ApplyBurnEffect(float duration, int ticksPerSecond, float totalDamage)
    {
        float reducedDamage = GetValueAfterResistance(totalDamage, ElementalDamageType.Fire);

        entity.ApplyDamagePerTick(duration, ticksPerSecond, reducedDamage);


        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        burnCoroutine = StartCoroutine(ApplyEffectVFXCoroutine(duration, ElementalDamageType.Fire));
    }

    public void ApplyElectrifyEffect(float charge, float damageOnFullCharge)
    {
    }

    private IEnumerator ApplyEffectVFXCoroutine(float duration, ElementalDamageType element)
    {
        currentElement = element;

        entityVFX.PlayOnStatusVFX(duration, element);

        yield return new WaitForSeconds(duration);

        currentElement = ElementalDamageType.None;
    }
}
