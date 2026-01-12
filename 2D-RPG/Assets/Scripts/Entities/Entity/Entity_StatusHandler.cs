using System.Collections;
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

    [SerializeField] private float currentLightningCharge = 0.0f;
    private float maxLightningCharge = 1.0f;

    // TODO: Will have a better status bar abstraction soon
    private Slider statusBar;
    [SerializeField] private GameObject statusBarObject;
    private Image statusBarImage;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();

        // TODO: Have the same code in Entity_Health, figure out abstraction. Same for SetCharge/UpdateStatusBar/etc
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

    private void SetCharge(float charge)
    {
        currentLightningCharge = charge;
        UpdateStatusBar();
    }

    private void UpdateStatusBar()
    {
        if (statusBar == null) return;

        statusBar.value = currentLightningCharge / maxLightningCharge;

        if (statusBar.value <= 0.0f)
        {
            statusBarObject.SetActive(false);
        } else
        {
            statusBarObject.SetActive(true);
        }
    }

    private void UpdateStatusBarColor(Color color)
    {
        statusBarImage.color = color;
    }

    public bool CanApply(ElementalDamageType element)
    {
        if (element == ElementalDamageType.Lightning && currentElement == ElementalDamageType.Lightning) return true;

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

    public void ApplyBurnEffect(float duration, int ticksPerSecond, float totalDamage)
    {
        float reducedDamage = GetValueAfterResistance(totalDamage, ElementalDamageType.Fire);

        StartCoroutine(BurnEffectCoroutine(duration, ticksPerSecond, reducedDamage));
    }

    private IEnumerator BurnEffectCoroutine(float duration, int ticksPerSecond, float totalDamage)
    {
        currentElement = ElementalDamageType.Fire;
        entityVFX.PlayOnStatusVFX(duration, currentElement);

        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1.0f / ticksPerSecond;

        for (int i = 0; i < tickCount; ++i)
        {
            entityHealth.ReduceHP(damagePerTick);

            yield return new WaitForSeconds(tickInterval);
        }

        currentElement = ElementalDamageType.None;
    }

    public void ApplyElectrifyEffect(float duration, float charge, float damageOnFullCharge)
    {
        float reducedCharge = GetValueAfterResistance(charge, ElementalDamageType.Lightning);
        UpdateStatusBarColor(entityVFX.lightningColor);
        SetCharge(currentLightningCharge + reducedCharge);

        if (currentLightningCharge >= maxLightningCharge)
        {
            Instantiate(entityVFX.lightningVFX, transform.position, Quaternion.identity);

            entityHealth.ReduceHP(damageOnFullCharge);

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
}
