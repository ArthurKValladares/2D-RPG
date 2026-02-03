using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainExpansion;

    private float maximumSizeScale;
    private float transformationDuration;
    private float domainDuration;
    private float slowDownPercentage;

    private float targetScale;
    private float startTransformationTime;

    private bool isShrinking;

    public void SetupDomain(Skill_DomainExpansion domainExpansion)
    {
        this.domainExpansion = domainExpansion;

        this.maximumSizeScale = domainExpansion.maximumSizeScale;
        this.transformationDuration = domainExpansion.transformationDuration;
        this.domainDuration = domainExpansion.GetDomainDuration();
        this.slowDownPercentage = domainExpansion.GetSlowDownPercentage();

        targetScale = maximumSizeScale;
        startTransformationTime = Time.time;

        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        sr.color = domainExpansion.GetDomainColor();

        Invoke(nameof(ShrinkDomain), domainDuration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float elapsedTime = Time.time - startTransformationTime;

        if (elapsedTime < transformationDuration)
        {
            Vector3 finalScaleVector = new Vector3(targetScale, targetScale, targetScale);
            transform.localScale = Vector3.Lerp(transform.localScale, finalScaleVector, elapsedTime / transformationDuration);
        }

        if (elapsedTime >= transformationDuration && isShrinking)
        {
            domainExpansion.ClearTargets();
            Destroy(gameObject);
        }
    }

    private void ShrinkDomain()
    {
        targetScale = 0.0f;
        startTransformationTime = Time.time;
        
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy)
        {
            domainExpansion.AddTarget(enemy);
            enemy.SlowDownEntityBy(domainDuration, slowDownPercentage, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy)
        {
            enemy.StopSlowDown();
        }
    }
}
