using System.Collections.Generic;
using UnityEngine;

public class ViolationManager : MonoBehaviour
{
    public static ViolationManager Instance;

    public int startingScore = 100;
    public int currentScore;

    public List<string> violations = new List<string>();
    private HashSet<string> activeViolations = new HashSet<string>();

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

    public void ClearViolation(string violationName)
    {
        activeViolations.Remove(violationName);
    }


    void FailExam()
    {
        Debug.Log("❌ Driving Test Failed");
        // show UI, stop car, end exam
    }
}
