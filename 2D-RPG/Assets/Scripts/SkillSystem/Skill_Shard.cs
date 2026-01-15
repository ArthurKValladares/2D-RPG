using UnityEngine;

public class Skill_Shard : Skill_Base
{
    [SerializeField] private GameObject shardObject;
    [SerializeField] private float detonationTime;

    public void CreateShard()
    {
        GameObject shardObj = Instantiate(shardObject, transform.position, Quaternion.identity);
        SkillObject_Shard shard = shardObj.GetComponent<SkillObject_Shard>();

        shard.SetupShardToExplode(detonationTime);
    }
}
