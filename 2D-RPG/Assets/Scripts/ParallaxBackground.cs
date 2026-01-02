using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] parallaxLayers;

    private float lastCameraPosX;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        lastCameraPosX = mainCamera.transform.position.x;

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.CalculateBackgroundWidth();
        }
    }

    private void FixedUpdate()
    {
        float currCameraPosX = mainCamera.transform.position.x;
        float cameraMoveDist = currCameraPosX - lastCameraPosX;

        float cameraLeftEdge = currCameraPosX - cameraHalfWidth;
        float cameraRightEdge = currCameraPosX + cameraHalfWidth;

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.MoveX(cameraMoveDist);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }

        lastCameraPosX = currCameraPosX;
    }
}
