using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.onFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        entity.onFlipped -= HandleFlip;
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
