using System;
using UnityEngine;

public enum NodeDirection
{
    None,
    Left,
    Right,
    Up,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
}

[Serializable]
public class UI_TreeConnectionDetails
{
    public NodeDirection direction;
    [Range(0.0f, 350.0f)]
    public float length;

    public UI_TreeConnectionHandler childNode;
}

public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectionDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (details.Length != connections.Length)
        {
            Debug.LogWarning("connections and details should have the same length: " + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            UI_TreeConnectionDetails detail = details[i];
            UI_TreeConnection connection = connections[i];

            connection.DirectConnection(detail.direction, detail.length);
            if (detail.childNode != null)
            {
                Vector2 targetPosition = connection.GetConnectionPoint(myRect);
                detail.childNode.SetPosition(targetPosition);
            }
        }
    }

    public void SetPosition(Vector2 position)
    {
        myRect.anchoredPosition = position;
    }
}
