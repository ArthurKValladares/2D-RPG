using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;

    [Header("Throw Details")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float throwForce = 5.0f;
    [SerializeField] private float comebackSpeed = 20.0f;

    [Header("Pierce Upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int pierceAmount = 2;

    [Header("Trajectory Prediction")]
    [SerializeField] private GameObject trajectoryPredictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = 0.05f;
    private Transform[] dots;
    private float swordGravity;

    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();

        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        if (currentSword != null)
        {
            currentSword.SendSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void EnableDots(bool enabled)
    {
        foreach (Transform t in dots)
        {
            t.gameObject.SetActive(enabled);
        }
    }

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < numberOfDots; ++i)
        {
            float t = i * spaceBetweenDots;
            dots[i].position = GetTrajectoryPoint(direction, t);
        }
    }

    public void ConfirmTrajectory(Vector2 direction)
    {
        confirmedDirection = direction;
    }

    private GameObject GetCorrectSword()
    {
        if (upgradeType == SkillUpgradeType.SwordThrow)
        {
            return swordPrefab;
        }

        if (upgradeType == SkillUpgradeType.SwordThrow_Pierce)
        {
            return pierceSwordPrefab;
        }

        Debug.LogError($"Uninplemented Sword Throw prefab for upgrade {upgradeType}");
        return swordPrefab;
    }
    public void ThrowSword()
    {
        GameObject newSword = Instantiate(GetCorrectSword(), dots[1].position, Quaternion.identity);
        currentSword = newSword.GetComponent<SkillObject_Sword>();

        currentSword.SetupSword(this, GetThrowForceInDirection(confirmedDirection));
    }

    private float GetScaledThrowForce()
    {
        return throwForce * 10.0f;
    }

    private Vector2 GetThrowForceInDirection(Vector2 direction)
    {
        return direction * GetScaledThrowForce();
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        Vector2 initialVelocity = GetThrowForceInDirection(direction);

        // The formula for the position of an object under constant acceleration is:
        // position(t) = 1/2 * acc_force * t^2
        // It comes from:
        // acceleration(t) = acc_force - since its constant, no relation to t
        // Then, since acceleration is the change in velocity over time, we can integrate over t to get velocity
        // velocity(t) = acc_force * t + C
        // Assuming we start at rest (C = 0)
        // velocity(t) = acc_force * t
        // Since velocity is the change in position over time, integrating again over time we get
        // position(t) = 1/2 * acc_force * t^2 + C
        // Assuming we start at position 0 (C = 0), then we get
        // position(t) = 1/2 * acc_force * t^2
        Vector2 gravityEffect = (t * t) * 0.5f * (Physics2D.gravity * swordGravity);

        Vector2 predictedPoint = initialVelocity * t + gravityEffect;
        Vector2 playerPosition = transform.root.position;

        return predictedPoint + playerPosition;
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; ++i)
        {
            newDots[i] = Instantiate(trajectoryPredictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
