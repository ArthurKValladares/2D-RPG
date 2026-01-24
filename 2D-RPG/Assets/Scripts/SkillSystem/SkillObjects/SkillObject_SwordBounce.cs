using UnityEngine;
using System.Collections.Generic;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] private float bounceRadius;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private int bounceCount;

    private Collider2D[] bounceTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();

    public override void SetupSword(Skill_SwordThrow swordThrow, Vector2 throwForce)
    {
        base.SetupSword(swordThrow, throwForce);

        if (anim)
        {
            anim.SetTrigger("spin");
        }

        bounceRadius = swordThrow.bounceRadius;
        bounceSpeed = swordThrow.bounceSpeed;
        bounceCount = swordThrow.bounceCount;
    }

    private void HandleBounce()
    {
        if (nextTarget == null) return;

        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, nextTarget.position);

        if (distance <= damageRadius)
        {
            DamageEnemiesInRadius(transform, damageRadius);
            BounceToNextTarget();

            if (bounceCount == 0 || nextTarget ==  null)
            {
                nextTarget = null;
                SendSwordBackToPlayer();
            }
        }
    }

    protected override void Update()
    {
        HandleComeback();
        HandleBounce();
    }

    //
    // TODO: Use a filter fuctions instead?
    //
    private List<Transform> GetAliveTargets()
    {
        List<Transform> targets = new List<Transform>();

        foreach (var target in bounceTargets)
        {
            if (target)
            {
                targets.Add(target.transform);
            }
        }

        return targets;
    }

    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTargets();

        foreach (Transform target in aliveTargets) {
            if (target && !selectedBefore.Contains(target))
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count > 0)
        {
            return validTargets;
        } else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }
    }
    //
    //
    //

    private Transform GetNextTarget()
    {
        List<Transform> validTargets = GetValidTargets();

        int idx = Random.Range(0, validTargets.Count);
        Transform nextTarget = validTargets[idx];

        selectedBefore.Add(nextTarget);

        return nextTarget;
    }

    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        --bounceCount;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (bounceTargets == null)
        {
            bounceTargets = EnemiesAround(transform, bounceRadius);
            rb.simulated = false;
        }

        DamageEnemiesInRadius(transform, damageRadius);

        if (bounceTargets.Length <= 1 || bounceCount == 0)
        {
            SendSwordBackToPlayer();
        } else
        {
            nextTarget = GetNextTarget();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.coral;
        Gizmos.DrawWireSphere(transform.position, bounceRadius);
    }
}
