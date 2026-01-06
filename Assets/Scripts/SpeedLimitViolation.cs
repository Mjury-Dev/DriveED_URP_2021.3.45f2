using UnityEngine;
using Ezereal;

public class SpeedLimitViolation : MonoBehaviour
{
    public float speedLimitKPH = 40f;
    public float toleranceKPH = 2f;
    public int penaltyPerSecond = 10;
    public float gracePeriodSeconds = 3f;

    public EzerealCarController car;

    private float violationTimer = 0f;
    private bool isPenalizing = false;
    private bool hasFailed = false;
    private float logCooldown = 0f;

    void Update()
    {
        if (car == null || ViolationManager.Instance == null || hasFailed) return;

        float limitWithTolerance = speedLimitKPH + toleranceKPH;
        float speed = car.currentSpeedKPH;

        if (speed > limitWithTolerance)
        {
            violationTimer += Time.deltaTime;

            if (violationTimer >= gracePeriodSeconds && !isPenalizing)
            {
                isPenalizing = true;
                ViolationManager.Instance.AddViolationOnce("Overspeeding Started", 5);
            }

            if (isPenalizing)
            {
                int framePenalty = Mathf.RoundToInt(penaltyPerSecond * Time.deltaTime);
                ViolationManager.Instance.currentScore -= framePenalty;

                logCooldown -= Time.deltaTime;
                if (logCooldown <= 0f)
                {
                    Debug.Log($"Overspeeding Penalty (-{framePenalty})");
                    Debug.Log($"Score: {ViolationManager.Instance.currentScore}");
                    logCooldown = 1f;
                }

                if (ViolationManager.Instance.currentScore <= 0 && !hasFailed)
                {
                    hasFailed = true;
                    ViolationManager.Instance.AddViolation("Overspeeding", 0);
                }
            }
        }
        else
        {
            if (isPenalizing && !hasFailed)
            {
                ViolationManager.Instance.AddViolation("Overspeeding Ended", 0);
                ViolationManager.Instance.ClearViolation("Overspeeding Started");
            }

            violationTimer = 0f;
            isPenalizing = false;
            hasFailed = false;
        }
    }
}
