using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    private Skill_Shard shardSKill;

    [Header("Explosion Data")]
    [SerializeField] private GameObject vfxGameObject;
    [SerializeField] private Color explosionColor = Color.white;

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

    private void SetupDamageInfo(Skill_Base skill)
    {
        playerStats = skill.player.stats;
        damageScaleData = skill.damageScaleData;
        primaryElementalDamage = skill.primaryElementalDamage;
        secondaryElementalDamage = skill.secondaryElementalDamage;
    }

    public void SetupShardToExplode(Skill_Shard shardSKill)
    {
        this.shardSKill = shardSKill;
        SetupDamageInfo(shardSKill);

        Invoke(nameof(Explode), shardSKill.GetDetonationTime());
    }

    public void SetupToMoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
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

        OnExplode?.Invoke();

        Destroy(gameObject);
    }
}
