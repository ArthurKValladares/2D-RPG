using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemyVFX;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<Enemy_VFX>();
    }

    private void StartCounterWindow()
    {
        enemy.EnableCounterWindow(true);
        enemyVFX.EnableAttackAlert(true);
    }

    private void EndCounterWindow()
    {
        enemy.EnableCounterWindow(false);
        enemyVFX.EnableAttackAlert(false);
    }
}
