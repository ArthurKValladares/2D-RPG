using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Enemy enemy)
        : base(enemy, "dead")
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.sm.SwitchOff();
        enemy.rb.simulated = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateEnded)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
