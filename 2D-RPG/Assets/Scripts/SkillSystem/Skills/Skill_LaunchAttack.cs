using UnityEngine;

public class Skill_LaunchAttack : Skill_Base
{
    [SerializeField] public Vector2 force = new(8.0f, 15.0f);
    [SerializeField] public float duration = 3.0f;
}
