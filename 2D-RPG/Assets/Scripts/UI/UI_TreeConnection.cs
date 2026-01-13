using UnityEngine;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(NodeDirection direction, float length)
    {
        bool shouldBeActive = direction != NodeDirection.None;

        float finalLength = shouldBeActive ? length : 0.0f;
        float angle = GetConnectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect.parent as RectTransform,
            childNodeConnectionPoint.position,
            null,
            out localPosition
        );
        return localPosition;
    }

    private float GetConnectionAngle(NodeDirection nodeDirection)
    {
        return nodeDirection switch
        {
            NodeDirection.Left => 180.0f,
            NodeDirection.Right => 0.0f,
            NodeDirection.Up => 90.0f,
            NodeDirection.Down => -90.0f,
            NodeDirection.UpLeft => 135.0f,
            NodeDirection.UpRight => 45.0f,
            NodeDirection.DownLeft => -135.0f,
            NodeDirection.DownRight => -45.0f,
            _ => 0.0f,
        };
    }
}
