using Unity.Cinemachine;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;

    [Header("Positioning Details")]
    [SerializeField] private Vector2 offsetsFromEdge;
    private float xTotalOffset;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        xTotalOffset = rect.sizeDelta.x / 2.0f + offsetsFromEdge.x;
    }

    public virtual void ShowTooltip(bool show, RectTransform targetRect)
    {
        if (show)
        {
            UpdatePosition(targetRect);
        } else if (!show)
        {
            rect.position = new Vector2(int.MaxValue, int.MaxValue);
        }
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2.0f;
        
        float targetX = targetRect.position.x;
        if (targetX > screenCenterX)
        {
            targetX -= xTotalOffset;
        } else
        {
            targetX += xTotalOffset;
        }

        float screenTop = Screen.height;
        float maxTooltipTop = screenTop - offsetsFromEdge.y;

        float screenBottom = 0.0f;
        float minToolTipBottom = screenBottom + offsetsFromEdge.y;

        float toolTipHalfHeight = rect.sizeDelta.y / 2.0f;
        float tooltipTop = targetRect.position.y + toolTipHalfHeight;
        float tooltipBottom = targetRect.position.y - toolTipHalfHeight;

        float targetY = targetRect.position.y;
        if (tooltipTop > maxTooltipTop)
        {
            targetY = screenTop - (toolTipHalfHeight + offsetsFromEdge.y);
        } else if (tooltipBottom < minToolTipBottom)
        {
            targetY = screenBottom + (toolTipHalfHeight + offsetsFromEdge.y);
        }

        rect.position = new Vector2(targetX, targetY);
    }
}
