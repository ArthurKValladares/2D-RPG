using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Parry Details")]
    public float parryRecoveryDuration = 0.2f;

    public bool PerformParry()
    {
        bool performed = false;

        foreach (Collider2D target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable != null)
            {
                performed |= counterable.HandleCounter();
            }
        }

        return performed;
    }
}
