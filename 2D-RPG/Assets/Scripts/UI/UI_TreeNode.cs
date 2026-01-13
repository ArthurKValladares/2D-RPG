using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;

    [SerializeField] private Skill_DataSO skillDataSO;

    [Header("Skill Details")]
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;

    [Header("Locked Skill Display Details")]
    [SerializeField] private string skillLockedColorHex = "#6E6E6E";
    private Color skillLockedColor;
    [SerializeField]  private float highlightIntensity = 1.5f;

    private bool isLearned;
    private bool isLocked;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        skillLockedColor = GetColorByHex(skillLockedColorHex);

        SetColor(skillLockedColor);    
    }

    private void Learn()
    {
        isLearned = true;
        SetColor(Color.white);
    }

    private bool CanBeLearned()
    {
        if (isLocked || isLearned) return false;
        return true;
    }

    private void SetColor(Color color)
    {
        if (!skillIcon) return;

        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeLearned())
        {
            Learn();
        }
        else
        {
            Debug.Log("cannot be learned");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowTooltip(true, rect, skillDataSO);

        if (!isLearned)
        {
            SetColor(skillLockedColor * highlightIntensity);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowTooltip(false, null);

        if (!isLearned)
        {
            SetColor(skillLockedColor);
        }
    }

    private Color GetColorByHex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    private void OnValidate()
    {
        if (skillDataSO == null)
        {
            return;
        }

        skillName = skillDataSO.skillName;
        skillIcon.sprite = skillDataSO.icon;
        gameObject.name = "UI Tree Node - " + skillName;
    }
}
