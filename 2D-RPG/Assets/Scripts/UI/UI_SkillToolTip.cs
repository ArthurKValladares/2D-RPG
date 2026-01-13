using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillRequirements;

    public override void ShowTooltip(bool show, RectTransform targetRect)
    {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, Skill_DataSO skillData)
    {
        ShowTooltip(show, targetRect);

        if (!show) return;

        skillName.text = skillData.name;
        skillDescription.text = skillData.description;
        skillRequirements.text = "Requirements:\n" +
            "- " + skillData.cost + " Skill Point(s).";
    }
}
