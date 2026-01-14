using System;
using UnityEngine;
using UnityEngine.UI;

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
    [Range(-50.0f, 50.0f)]
    public float rotation;

    public UI_TreeConnectionHandler childNode;
}

public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectionDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if (connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
    }

    public void SetPosition(Vector2 position)
    {
        myRect.anchoredPosition = position;
    }

    public void SetConnectionImage(Image image)
    {
        connectionImage = image;
    }

    public void ConnectionImageLearned(bool learned)
    {
        if (connectionImage == null) return;

        connectionImage.color = learned ? Color.white : originalColor;
    }

    private void OnValidate()
    {
        if (details.Length != connections.Length)
        {
            Debug.LogWarning("connections and details should have the same length: " + gameObject.name);
            return;
        }

        UpdateConnections();
    }

    public void UpdateConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            UI_TreeConnectionDetails detail = details[i];
            UI_TreeConnection connection = connections[i];

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);
            if (detail.childNode != null)
            {
                detail.childNode.SetPosition(connection.GetConnectionPoint(myRect));
                detail.childNode.SetConnectionImage(connection.GetConnectionImage());
                detail.childNode.transform.SetAsLastSibling();
            }
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach (UI_TreeConnectionDetails detail in details)
        {
            if (detail.childNode == null) continue;

            detail.childNode.UpdateConnections();
        }
    }
}
