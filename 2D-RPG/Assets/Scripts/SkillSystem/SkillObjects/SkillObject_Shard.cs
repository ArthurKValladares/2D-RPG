using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    private Skill_Shard shardSKill;

    [Header("Explosion Data")]
    [SerializeField] private GameObject vfxGameObject;

    private Transform target;
    private float speed;

    private void Update()
    {
        MoveTowardsClosestEnemy();
    }

    private void MoveTowardsClosestEnemy()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }


    public void SetupShardToExplode(Skill_Shard shardSKill)
    {
        this.shardSKill = shardSKill;

        SetupSkillData(shardSKill);

        Invoke(nameof(Explode), shardSKill.GetDetonationTime());
    }

    public void SetupToMoveTowardsTarget(Transform newTarget = null)
    {
        this.speed = shardSKill.GetSpeed();

        this.target = newTarget != null ? newTarget : FindClosestTarget();
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
        explosionSR.color = shardSKill.player.playerVFX.GetOnHitVFXColor(primaryElementalDamage);

        OnExplode?.Invoke();

        Destroy(gameObject);
    }
}
