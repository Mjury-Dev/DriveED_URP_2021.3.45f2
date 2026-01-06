using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ViolationManager : MonoBehaviour
{
    public static ViolationManager Instance;

    public int startingScore = 100;
    public int currentScore;

    public List<string> violations = new List<string>();
    private HashSet<string> activeViolations = new HashSet<string>();
    private HashSet<string> runningPenalties = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentScore = startingScore;
    }

    public void AddViolation(string violationName, int penalty)
    {
        violations.Add(violationName);
        currentScore -= penalty;

        Debug.Log($"Violation: {violationName} (-{penalty})");
        Debug.Log($"Score: {currentScore}");

        if (currentScore <= 0)
        {
            FailExam();
        }
    }

    public void AddViolationOnce(string violationName, int penalty)
    {
        if (activeViolations.Contains(violationName))
            return;

        activeViolations.Add(violationName);
        AddViolation(violationName, penalty);
    }

    public void StartContinuousPenalty(string violationName, int penaltyPerSecond)
    {
        if (runningPenalties.Contains(violationName))
            return;

        runningPenalties.Add(violationName);
        StartCoroutine(ContinuousPenaltyTick(violationName, penaltyPerSecond));
    }

    private IEnumerator ContinuousPenaltyTick(string violationName, int penaltyPerSecond)
    {
        while (activeViolations.Contains(violationName))
        {
            currentScore -= penaltyPerSecond;
            Debug.Log($"Violation: {violationName} (-{penaltyPerSecond}/sec)");
            Debug.Log($"Score: {currentScore}");

            if (currentScore <= 0)
            {
                FailExam();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }

        runningPenalties.Remove(violationName);
    }

    public void ClearViolation(string violationName)
    {
        activeViolations.Remove(violationName);
    }

    void FailExam()
    {
        Debug.Log("Driving Test Failed");
        // TODO: Stop vehicle, freeze controls, show fail UI
    }
}
