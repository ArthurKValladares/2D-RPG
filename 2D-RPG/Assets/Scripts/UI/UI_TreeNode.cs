using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;

    [Header("Skill Details")]
    [SerializeField] public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;

    [Header("Unlock Details")]
    public UI_TreeNode[] neededTreeNodes;
    public UI_TreeNode[] conflictNodes;
    private bool isLearned;
    private bool isLocked;

    [Header("Locked Skill Display Details")]
    [SerializeField] private string skillLockedColorHex = "#6E6E6E";
    [SerializeField] private Color skillLockedColor;
    [SerializeField] private float highlightIntensity = 1.5f;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();

        skillLockedColor = Helpers.GetColorByHex(skillLockedColorHex);

        SetColor(skillLockedColor);    
    }

    public bool IsLearned()
    {
        return isLearned;
    }

    private void Learn()
    {
        isLearned = true;
        skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictingSkills();

        SetColor(Color.white);
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    private bool CanBeLearned()
    {
        if (isLocked || isLearned) return false;

        if (!skillTree.HasEnoughSkillPoints(skillData.cost))
        {
            return false;
        }

        foreach (UI_TreeNode node in neededTreeNodes)
        {
            if (!node.IsLearned())
            {
                return false;
            }
        }

        foreach (UI_TreeNode node in conflictNodes)
        {
            if (node.IsLearned())
            {
                return false;
            }
        }

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
        ui.skillToolTip.ShowTooltip(true, rect, this);

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

    private void LockConflictingSkills()
    {
        foreach (UI_TreeNode node in conflictNodes)
        {
            node.SetLocked(true);
        }
    }

    private void OnValidate()
    {
        if (skillData == null)
        {
            return;
        }

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI Tree Node - " + skillName;
    }
}
