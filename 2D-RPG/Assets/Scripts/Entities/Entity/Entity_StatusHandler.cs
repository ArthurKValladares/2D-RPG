using System.Collections;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;

    private ElementalDamageType currentElement = ElementalDamageType.None;

    private Coroutine electrifyCoroutine;
    private float currentLightningCharge = 0.0f;

    // TODO: Will have a better status bar abstraction soon
    private Slider statusBar;
    public GameObject statusBarObject;
    private Image statusBarImage;

    // Constants
    const float MAX_ELECTRIFY_CHARGE = 1.0f;
    const int BURN_TICKS_PER_SECOND = 4;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();

        // TODO: I have the same code in Entity_Health, figure out abstraction. Same for SetCharge/UpdateStatusBar/etc
        Slider[] sliders = GetComponentsInChildren<Slider>();
        foreach (Slider slider in sliders)
        {
            if (slider.CompareTag("StatusBar"))
            {
                statusBar = slider;
                break;
            }
        }
        statusBarImage = statusBar.fillRect.GetComponent<Image>();
        
        SetCharge(0.0f);
    }

    public bool CanApply(ElementalDamageType element)
    {
        if (element == ElementalDamageType.Lightning && currentElement == ElementalDamageType.Lightning) return true;

        return currentElement == ElementalDamageType.None;
    }

    public void ApplyStatusEffect(ElementalDamageType element, ElementalEffectData data)
    {
        if (!CanApply(element)) return;

        switch (element)
        {
            case ElementalDamageType.Ice:
                ApplyChillEffect(data.chillDuration, data.chillSlowPercentage);
                break;
            case ElementalDamageType.Fire:
                ApplyBurnEffect(data.burnDuration, BURN_TICKS_PER_SECOND, data.burnTotalDamage);
                break;
            case ElementalDamageType.Lightning:
                ApplyElectrifyEffect(data.electrifyDuration, data.electrifyCharge, data.electrifyDamageOnFullCharge);
                break;
        }
    }

    private float GetValueAfterResistance(float val, ElementalDamageType element)
    {
        float resistance = entityStats.GetElementalResistance(element);
        return val * (1.0f - resistance);
    }

    public void RemoveAllNegativeEffects()
    {
        StopAllCoroutines();
        currentElement = ElementalDamageType.None;
        entityVFX.StopAllVFX();
        currentLightningCharge = 0.0f;
    }

    //
    // Chill
    //

    private void ApplyChillEffect(float duration, float slowPercentage)
    {
        float reducedDuration = GetValueAfterResistance(duration, ElementalDamageType.Ice);

        StartCoroutine(ChillEffectCoroutine(reducedDuration, slowPercentage));
    }

    private IEnumerator ChillEffectCoroutine(float duration, float slowPercentage)
    {
        currentElement = ElementalDamageType.Ice;
        entityVFX.PlayOnStatusVFX(duration, currentElement);

        entity.SlowDownEntityBy(duration, slowPercentage);
        

        yield return new WaitForSeconds(duration);

        currentElement = ElementalDamageType.None;
    }

    //
    // Burn
    //

    private void ApplyBurnEffect(float duration, int ticksPerSecond, float totalDamage)
    {
        float reducedDamage = GetValueAfterResistance(totalDamage, ElementalDamageType.Fire);
        Debug.Log($"Stat burn effect for {totalDamage} in {duration} seconds.");

        StartCoroutine(BurnEffectCoroutine(duration, ticksPerSecond, reducedDamage));
    }

    private IEnumerator BurnEffectCoroutine(float duration, int ticksPerSecond, float totalDamage)
    {
        currentElement = ElementalDamageType.Fire;
        entityVFX.PlayOnStatusVFX(duration, currentElement);

        float tickInterval = 1.0f / ticksPerSecond;
        int tickCount = (int) Mathf.Floor(ticksPerSecond * duration);
        float damagePerTick = totalDamage / tickCount;

        for (int i = 0; i < tickCount; ++i)
        {
            entityHealth.ReduceHP(damagePerTick);
            Debug.Log($"Tick burn for {damagePerTick}.");

            yield return new WaitForSeconds(tickInterval);
        }

        currentElement = ElementalDamageType.None;
    }

    //
    // Electrify
    //

    private void SetCharge(float charge)
    {
        currentLightningCharge = charge;
        UpdateStatusBar();
    }

    private void ApplyElectrifyEffect(float duration, float charge, float damageOnFullCharge)
    {
        float reducedCharge = GetValueAfterResistance(charge, ElementalDamageType.Lightning);
        UpdateStatusBarColor(entityVFX.lightningColor);
        SetCharge(currentLightningCharge + reducedCharge);

        if (currentLightningCharge >= MAX_ELECTRIFY_CHARGE)
        {
            Instantiate(entityVFX.lightningVFX, transform.position, Quaternion.identity);

            entityHealth.ReduceHP(damageOnFullCharge);
            Debug.Log($"trigger electrify for {damageOnFullCharge}.");

            StopElectrifyEffect();
            return;
        }

        if (electrifyCoroutine != null)
        {
            StopCoroutine(electrifyCoroutine);
        }
        electrifyCoroutine = StartCoroutine(ElectrifyEffectCoroutine(duration));
    }

    private IEnumerator ElectrifyEffectCoroutine(float duration)
    {
        currentElement = ElementalDamageType.Lightning;
        entityVFX.PlayOnStatusVFX(duration, currentElement);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    private void StopElectrifyEffect()
    {
        currentElement = ElementalDamageType.None;
        SetCharge(0.0f);
        entityVFX.StopAllVFX();
    }

    //
    // Status Bar
    //

    public void SetStatusBarVisible(bool visible)
    {
        if (statusBar == null) return;

        statusBarObject.SetActive(visible);
    }

    private void UpdateStatusBar()
    {
        if (statusBar == null) return;

        statusBar.value = currentLightningCharge / MAX_ELECTRIFY_CHARGE;

        if (statusBar.value <= 0.0f)
        {
            SetStatusBarVisible(false);
        }
        else
        {
            SetStatusBarVisible(true);
        }
    }

    private void UpdateStatusBarColor(Color color)
    {
        statusBarImage.color = color;
    }
}
