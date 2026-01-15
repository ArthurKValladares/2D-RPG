using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] GameObject imageEchoPreFab;
    private Coroutine imageEchoCoroutine;

    public void DoImageEchoVFX(float duration)
    {
        if (imageEchoCoroutine != null)
        {
            StopCoroutine(imageEchoCoroutine);
        }

        imageEchoCoroutine = StartCoroutine(ImageEchoEffectCo(duration));
    }

    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float time = 0.0f;

        while (time < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);

            time += imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPreFab, transform.position, transform.rotation);
        SpriteRenderer echo_sr = imageEcho.GetComponentInChildren<SpriteRenderer>();
        echo_sr.sprite = sr.sprite;
    }
}
