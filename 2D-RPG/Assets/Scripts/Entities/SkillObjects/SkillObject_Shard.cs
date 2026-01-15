using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    [SerializeField] private GameObject vfxGameObject;
    [SerializeField] private Color explosionColor = Color.white;

    public void SetupShardToExplode(float detonationTime)
    {
        Invoke(nameof(Explode), detonationTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        Explode();
    }

    private void Explode()
    {
        DamageEnemiesInRadius(targetCheck, targetCheckRadius);

        GameObject explosionObject = Instantiate(vfxGameObject, transform.position, Quaternion.identity);
        SpriteRenderer explosionSR = explosionObject.GetComponentInChildren<SpriteRenderer>();
        explosionSR.color = explosionColor;

        Destroy(gameObject);
    }
}
