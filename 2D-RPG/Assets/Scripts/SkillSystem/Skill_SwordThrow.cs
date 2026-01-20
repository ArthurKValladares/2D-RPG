using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    [Header("Throw Details")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float throwForce = 5.0f;
    [SerializeField] private float swordGravity = 3.5f;

    [Header("Trajectory Prediction")]
    [SerializeField] private GameObject trajectoryPredictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = 0.05f;
    private Transform[] dots;

    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();
        dots = GenerateDots();
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

    public void ThrowSword()
    {
        Debug.Log("Threw Sword!");
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowForce = throwForce * 10.0f;

        Vector2 initialVelocity = direction * scaledThrowForce;

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
