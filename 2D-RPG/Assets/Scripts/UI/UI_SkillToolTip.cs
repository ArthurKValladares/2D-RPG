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
    [SerializeField] private string importantInfoHex;
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
        skillTree = ui.GetComponentInChildren<UI_SkillTree>();
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
        

        string lockedSkillString = Helpers.GetColoredText(importantInfoHex, lockedOutText);
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
        sb.AppendLine(Helpers.GetColoredText(costColor, $"- {skillCost} Skill Point(s)"));

        foreach (UI_TreeNode node in neededNodes)
        {
            string nodeColor = node.IsLearned()
                ? metConditionsHex
                : notMetConditionsHex;
            sb.AppendLine(Helpers.GetColoredText(nodeColor, $"- {node.skillData.skillName}"));
        }

        if (conflictNodes.Length > 0)
        {
            sb.AppendLine();
            sb.AppendLine(Helpers.GetColoredText(importantInfoHex, "Locks Out:"));
            foreach ( UI_TreeNode node in conflictNodes)
            {
                sb.AppendLine(Helpers.GetColoredText(importantInfoHex, $"- {node.skillData.skillName}"));
            }
        }

        return sb.ToString();
    }

    public void LockedSkillEffect()
    {
        if (textEffectCo != null)
        {
            StopCoroutine(textEffectCo);
        }

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements, blinkInterval, blinkCount));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = Helpers.GetColoredText(notMetConditionsHex, lockedOutText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = Helpers.GetColoredText(importantInfoHex, lockedOutText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
