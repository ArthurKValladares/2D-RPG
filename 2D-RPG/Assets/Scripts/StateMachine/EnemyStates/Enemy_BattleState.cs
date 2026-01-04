using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeDetectedPlayer;

    public Enemy_BattleState(Enemy enemy)
        : base(enemy, "battle")
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();
        
        if (!player)
        {
            player = enemy.TryGettingPlayerTransform();
        }

        if (ShouldRetreat())
        {
            enemy.rb.linearVelocity = new(enemy.retreateVelocity.x * -PlayerDirScale(), enemy.retreateVelocity.y);
            enemy.HandleFlip(PlayerDirScale());
        }
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D player_detection = enemy.PlayerDetection();
        if (player_detection)
        {
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
        {
            enemy.sm.ChangeState(enemy.idleState);
            return;
        }

        if (player_detection && WithinAttackRangeX())
        {
            enemy.sm.ChangeState(enemy.attackState);
        }
        else if (!enemy.wallsDetected && enemy.groundDetected)
        {
            enemy.SetVelocityX(enemy.battleMoveSpeed * PlayerDirScale());
        }
    }

    private void UpdateBattleTimer()
    {
        lastTimeDetectedPlayer = Time.time;
    }

    private bool BattleTimeIsOver()
    {
        return (Time.time - lastTimeDetectedPlayer) > enemy.battleTimeDuration;
    }

    private float DistanceToPlayerX()
    {
        if (player == null)
        {
            return float.MaxValue;
        } else
        {
            return Mathf.Abs(player.transform.position.x - enemy.transform.position.x);
        }
    }

    private bool WithinAttackRangeX()
    {
        return DistanceToPlayerX() <= enemy.attackDistance;
    }

    private float PlayerDirScale()
    {
        if (player == null) return 0;

        if (player.transform.position.x > enemy.transform.position.x)
        {
            return 1.0f;
        } else
        {
            return -1.0f;
        }
    }

    private bool ShouldRetreat()
    {
        return DistanceToPlayerX() < enemy.minRetreatDistance;
    }
}
