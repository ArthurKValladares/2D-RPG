using UnityEngine;

public enum SkillType
{
    Dash,
    TimeEcho,
    TimeShard,
}

public enum SkillUpgradeType
{
    None,

    // Dash Tree
    Dash,
    Dash_CloneOnStart,
    Dash_CloneOnStartAndArrival,
    Dash_ShardOnShart,
    Dash_ShardOnStartAndArrival,

    // Shard Tree
    Shard,
    Shard_MoveToEnemy,
    Shard_Multicast,
    Shard_Teleport,
    Shard_TeleportHpRewind
}