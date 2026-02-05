using UnityEngine;

public enum SkillType
{
    Dash,
    TimeEcho,
    Shard,
    SwordThrow,
    DomainExpansion,
    LaunchAttack
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
    Shard_TeleportHpRewind,

    // Sword throw tree
    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,

    // Time Echo tree
    TimeEcho,
    TimeEcho_SingleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_ChanceToMultiply,
    TimeEcho_HealWisp,
    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,

    // Domain Expansion tree
    Domain_SlowDown,
    Domain_EchoSpam,
    Domain_ShardSpam,

    // Launch Attack Tree
    LaunchAttack,
}