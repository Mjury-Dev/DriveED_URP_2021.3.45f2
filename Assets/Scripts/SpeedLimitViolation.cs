using UnityEngine;
using Ezereal;

public class SpeedLimitViolation : MonoBehaviour
{
    [Header("Speed Settings")]
    public float speedLimitKPH = 40f;
    public float toleranceKPH = 2f;

    [Header("Penalty Settings")]
    public int penaltyPerSecond = 10;
    public float gracePeriodSeconds = 3f;

    [Header("References")]
    public EzerealCarController car;

    private float violationTimer = 0f;
    private bool isPenalizing = false;
    private bool hasFailed = false;

    private float penaltyAccumulator = 0f;
    private float logCooldown = 0f;

    void Update()
    {
        if (car == null || ViolationManager.Instance == null || hasFailed)
            return;

        float limitWithTolerance = speedLimitKPH + toleranceKPH;
        float speed = car.currentSpeedKPH;

        if (speed > limitWithTolerance)
        {
            violationTimer += Time.deltaTime;

            // Start penalizing after grace period
            if (violationTimer >= gracePeriodSeconds && !isPenalizing)
            {
                isPenalizing = true;
                penaltyAccumulator = 0f;
                logCooldown = 0f;

                ViolationManager.Instance.AddViolationOnce("Overspeeding Started", 5);
            }

            if (isPenalizing)
            {
                // Accumulate penalty smoothly over time
                penaltyAccumulator += penaltyPerSecond * Time.deltaTime;

                int penaltyToApply = Mathf.FloorToInt(penaltyAccumulator);
                if (penaltyToApply > 0)
                {
                    ViolationManager.Instance.currentScore -= penaltyToApply;
                    penaltyAccumulator -= penaltyToApply;
                }

                // Log once per second
                logCooldown -= Time.deltaTime;
                if (logCooldown <= 0f)
                {
                    Debug.Log($"Overspeeding Penalty (-{penaltyToApply})");
                    Debug.Log($"Score: {ViolationManager.Instance.currentScore}");
                    logCooldown = 1f;
                }

                // Failure condition
                if (ViolationManager.Instance.currentScore <= 0)
                {
                    hasFailed = true;
                    ViolationManager.Instance.AddViolation("Overspeeding", 0);
                }
            }
        }
        else
        {
            // Speed back within limit
            if (isPenalizing && !hasFailed)
            {
                ViolationManager.Instance.AddViolation("Overspeeding Ended", 0);
                ViolationManager.Instance.ClearViolation("Overspeeding Started");
            }

            violationTimer = 0f;
            isPenalizing = false;
            penaltyAccumulator = 0f;
        }
    }
}
