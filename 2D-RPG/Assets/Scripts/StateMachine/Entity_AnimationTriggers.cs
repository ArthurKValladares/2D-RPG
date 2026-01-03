using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void CurrentStateEnded()
    {
        entity.SetCurrentStateEnded();
    }
}
