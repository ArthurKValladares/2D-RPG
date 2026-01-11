using System.Collections;
using UnityEngine;

public class BuffObject : MonoBehaviour
{

    private Vector3 originalPos;
    private SpriteRenderer sr;

    [Header("Buff Details")]
    private float buffDuration = 4.0f;
    private bool canBeUsed = true;

    [Header("Oscillation")]
    [SerializeField] private float oscillationSpeed = 3.0f;
    [SerializeField] private float yOscillationRange = 0.2f;

    private void Awake()
    {
        originalPos = transform.position;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float yBobOffset = Mathf.Sin(Time.time * oscillationSpeed) * yOscillationRange;
        transform.position = originalPos + new Vector3(0, yBobOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed) return;
        
        StartCoroutine(ApplyBuffCoroutine(collision));
    }

    private IEnumerator ApplyBuffCoroutine(Collider2D collision)
    {
        canBeUsed = false;
        sr.color = Color.clear;

        Entity_Stats stats = collision.GetComponent<Entity_Stats>();
        if (!stats)
        {
            yield return null;
        }

        float originalAttackSpeed = stats.offensiveStats.attackSpeedMultiplier.GetValue();
        stats.offensiveStats.attackSpeedMultiplier.SetValue(originalAttackSpeed + 0.2f);

        yield return new WaitForSeconds(buffDuration);

        stats.offensiveStats.attackSpeedMultiplier.SetValue(originalAttackSpeed);

        Destroy(gameObject);
    }
}
