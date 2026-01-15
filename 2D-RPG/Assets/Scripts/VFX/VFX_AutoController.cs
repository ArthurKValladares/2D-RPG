using System.Collections;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1.0f;

    [Header("Random Position")]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private Vector2 minOffset = new Vector2(-0.3f, -0.3f);
    [SerializeField] private Vector2 maxOffset = new Vector2(0.3f, 0.3f);
    [SerializeField] private bool randomRotation = true;
    [SerializeField] private float minRotationAngle = 0.0f;
    [SerializeField] private float maxRotationAngle = 360.0f;

    [Header("Fade Effect")]
    [SerializeField] private bool shouldFade = false;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (shouldFade)
        {
            StartCoroutine(FadeCo());
        }

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

    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        float time = 0.0f;

        while (targetColor.a > 0.0f)
        {
            float progress = time / fadeDuration;

            float alpha = Mathf.Lerp(1.0f, 0.0f, progress);
            targetColor.a = alpha;
            sr.color = targetColor;

            yield return null;

            time += Time.deltaTime;
        }

        sr.color = targetColor;
    }
}
