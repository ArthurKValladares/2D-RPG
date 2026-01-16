using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    [Header("Text Color Details")]
    [SerializeField] private string metConditionsHex;
    [SerializeField] private string notMetConditionsHex;
    private string originalNotMetConditionsHex;
    [SerializeField] private string importantInfoHex;
    private string originalImportantInfoHex;
    [SerializeField] private Color exampleColor;

    [Space]
    [SerializeField] private string lockedOutText = "You've taken a different path, this skill is now locked.";

    [Header("Lock Text Effect Details")]
    [SerializeField] private float blinkInterval = 0.15f;
    [SerializeField] private int blinkCount = 3;
    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);

        originalNotMetConditionsHex = notMetConditionsHex;
        originalImportantInfoHex = importantInfoHex;
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
        
        skillRequirements.text = PickRequirementsText(node);
    }

    private string PickRequirementsText(UI_TreeNode node)
    {
        string lockedSkillString = Helpers.GetColoredText(importantInfoHex, lockedOutText);
        string requirementsString = GetRequirements(node.skillData.cost, node.neededTreeNodes, node.conflictNodes);

        return node.IsLocked()
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
        sb.AppendLine(Helpers.GetColoredText(costColor, $"- {skillCost} Skill Point(s)"));

        foreach (UI_TreeNode node in neededNodes)
        {
            if (node == null) continue;

            string nodeColor = node.IsLearned()
                ? metConditionsHex
                : notMetConditionsHex;
            sb.AppendLine(Helpers.GetColoredText(nodeColor, $"- {node.skillData.skillName}"));
        }

        if (conflictNodes.Length > 0)
        {
            // NOTE: We always use the original colors in locks out since we don't want to be part of the blinking effect
            sb.AppendLine();
            sb.AppendLine(Helpers.GetColoredText(originalImportantInfoHex, "Locks Out:"));
            foreach ( UI_TreeNode node in conflictNodes)
            {
                if (node == null) continue;

                sb.AppendLine(Helpers.GetColoredText(originalImportantInfoHex, $"- {node.skillData.skillName}"));
            }
        }

        return sb.ToString();
    }

    public void HighlightNotMetRequirementsEffect(UI_TreeNode node)
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }

        textEffectCo = StartCoroutine(TextBlinkEffectCo(node, skillRequirements, blinkInterval, blinkCount));
    }

    private IEnumerator TextBlinkEffectCo(UI_TreeNode node, TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = PickRequirementsText(node);
            yield return new WaitForSeconds(blinkInterval);

            notMetConditionsHex = originalImportantInfoHex;
            importantInfoHex = originalNotMetConditionsHex;

            text.text = PickRequirementsText(node);
            yield return new WaitForSeconds(blinkInterval);

            notMetConditionsHex = originalNotMetConditionsHex;
            importantInfoHex = originalImportantInfoHex;
        }
    }
}
