using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1.0f;

    [Header("Random Position")]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private Vector2 minOffset = new Vector2(-0.3f, -0.3f);
    [SerializeField] private Vector2 maxOffset = new Vector2(0.3f, 0.3f);
    [SerializeField] private bool randomRotation = true;
    [SerializeField] private float minRotationAngle = 0.0f;
    [SerializeField] private float maxRotationAngle = 360.0f;

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        // TODO: Use animation event instead
        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void ApplyRandomOffset()
    {
        if (!randomOffset) return;

        float xOffset = Random.Range(minOffset.x, maxOffset.x);
        float yOffset = Random.Range(minOffset.y, maxOffset.y);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation()
    {
        if (!randomRotation) return;

        float zRotation = Random.Range(minRotationAngle, maxRotationAngle);

        transform.Rotate(0, 0, zRotation);
    }
}
