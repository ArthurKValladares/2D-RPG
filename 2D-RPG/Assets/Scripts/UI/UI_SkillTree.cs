using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectionHandler[] parentNodes;

    private void Start()
    {
        UpdateAllConnections();
    }

    public bool HasEnoughSkillPoints(int points)
    {
        return skillPoints >= points;
    }

    public void RemoveSkillPoints(int points)
    {
        if (skillPoints < points)
        {
            Debug.LogWarning("Not enough skill points. Requires " + points + " has " + skillPoints);
        }

        skillPoints = Mathf.Max(0, skillPoints - points);
    }

    public void AddSkillPoints(int points)
    {
        skillPoints += points;
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (UI_TreeConnectionHandler node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    [ContextMenu("Refund All")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] treeNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (UI_TreeNode node in treeNodes)
        {
            node.Refund();
        }
    }
}
