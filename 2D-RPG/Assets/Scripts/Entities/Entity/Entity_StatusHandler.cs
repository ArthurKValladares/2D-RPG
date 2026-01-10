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

    public void ApplyChillEffect(float duration, float slowPercentage)
    {
        float iceResistance = entityStats.GetElementalResistance(ElementalDamageType.Ice);
        float reducedDuration = duration * (1.0f - iceResistance);

        entity.SlowDownEntityBy(reducedDuration, slowPercentage);

        if (chillCoroutine != null)
        {
            StopCoroutine(chillCoroutine);
        }
        chillCoroutine = StartCoroutine(ApplyEffectCoroutine(reducedDuration, ElementalDamageType.Ice));
    }

    public void ApplyBurnEffect(float duration, float damagePerTick)
    {
    }

    public void ApplyElectrifyEffect(float charge, float damageOnFullCharge)
    {
    }

    private IEnumerator ApplyEffectCoroutine(float duration, ElementalDamageType element)
    {
        currentElement = element;

        entityVFX.PlayOnStatusVFX(duration, element);

        yield return new WaitForSeconds(duration);

        currentElement = ElementalDamageType.None;
    }
}
