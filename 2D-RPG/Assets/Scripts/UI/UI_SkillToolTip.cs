using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Space]
    [SerializeField] private string metConditionsHex;
    [SerializeField] private string notMetConditionsHex;
    [SerializeField] private string importantInfoHex;
    [SerializeField] private Color exampleColor;
    [Space]
    [SerializeField] private string lockedOutText = "You've taken a different path, this skill is now locked.";

    protected override void Awake()
    {
        base.Awake();

        skillTree = GetComponentInParent<UI_SkillTree>();
    }

    public override void ShowTooltip(bool show, RectTransform targetRect)
    {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, UI_TreeNode node)
    {
        ShowTooltip(show, targetRect);

        if (!show) return;

        skillName.text = node.skillData.name;
        skillDescription.text = node.skillData.description;
        

        string lockedSkillString = $"<color={importantInfoHex}>{lockedOutText}</color>";
        string requirementsString = GetRequirements(node.skillData.cost, node.neededTreeNodes, node.conflictNodes);

        skillRequirements.text = node.IsLocked()
            ? lockedSkillString
            : requirementsString;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");

        string costColor = skillTree.HasEnoughSkillPoints(skillCost)
            ? metConditionsHex
            : notMetConditionsHex;
        sb.AppendLine($"<color={costColor}>- {skillCost} Skill Point(s)</color>");

        foreach (UI_TreeNode node in neededNodes)
        {
            string nodeColor = node.IsLearned()
                ? metConditionsHex
                : notMetConditionsHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName}</color>");
        }

        if (conflictNodes.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine($"<color={importantInfoHex}>Locks Out:</color>");
            foreach ( UI_TreeNode node in conflictNodes)
            {
                sb.AppendLine($"<color={importantInfoHex}>- {node.skillData.skillName}</color>");
            }
        }

        return sb.ToString();
    }
}
