using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float duration;

    public override void TryToUseSkill()
    {
        if (CanUseSkill())
        {
            CreateTimeEcho();
        }
    }

    public float GetEchoDuration()
    {
        return duration;
    }

    private void CreateTimeEcho()
    {
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        SkillObject_TimeEcho timeEchoObj = timeEcho.GetComponent<SkillObject_TimeEcho>();
        timeEchoObj.SetupTimeEcho(this);
    }

    public void CreateRawTimeEcho()
    {
        CreateTimeEcho();
    }
}
