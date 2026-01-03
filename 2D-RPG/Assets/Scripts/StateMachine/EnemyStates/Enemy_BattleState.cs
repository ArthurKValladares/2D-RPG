using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    public Enemy_BattleState(Enemy enemy)
        : base(enemy, "battle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("BATTLE TIME!");
    }
}
