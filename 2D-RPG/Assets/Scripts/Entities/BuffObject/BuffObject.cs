using System;
using System.Collections;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class BuffInfo
{
    public BuffInfo(StatType ty, float value)
    {
        this.ty = ty; 
        this.value = value;
    }

    public StatType ty;
    public float value;
}

public class BuffObject : MonoBehaviour
{

    private Vector3 originalPos;
    private SpriteRenderer sr;

    [Header("Buff Details")]
    [SerializeField] private string buffName;
    [SerializeField] private BuffInfo[] buffs;
    [SerializeField] private float buffDuration = 4.0f;
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
            Debug.LogError("Could not find EntityStats in BuffObject collider!");
            yield return null;
        }

        foreach (BuffInfo info in buffs)
        {
            stats.GetStat(info.ty).AddModifier(buffName, info.value);
        }

        yield return new WaitForSeconds(buffDuration);

        foreach (BuffInfo info in buffs)
        {
            stats.GetStat(info.ty).RemoveModifier(buffName);
        }

        Destroy(gameObject);
    }
}
