using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float widthOffset = 10.0f;

    private float backgroundWidth;
    private float backgroundHalfWidth;

    public void CalculateBackgroundWidth()
    {
        backgroundWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        backgroundHalfWidth = backgroundWidth / 2.0f;
    }

    public void MoveX(float distanceToMoveX)
    {
        background.position += Vector3.right * (distanceToMoveX * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = (background.position.x - backgroundHalfWidth) + widthOffset;
        float imageRightEdge = (background.position.x + backgroundHalfWidth) - widthOffset;

        if (imageRightEdge < cameraLeftEdge)
        {
            background.position += Vector3.right * backgroundWidth;
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            background.position += Vector3.right * -backgroundWidth;
        }
    }
}
