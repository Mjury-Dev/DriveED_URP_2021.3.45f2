using TMPro;
using UnityEngine;
using System.Collections;

public class ViolationUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI violationText;

    private Coroutine clearCoroutine;
    private int lastViolationCount = 0;

    void Update()
    {
        if (ViolationManager.Instance == null)
            return;

        // Update score
        scoreText.text = $"Score: {ViolationManager.Instance.currentScore}";

        // Only update if violations changed
        int currentCount = ViolationManager.Instance.violations.Count;
        if (currentCount != lastViolationCount)
        {
            lastViolationCount = currentCount;
            UpdateViolationText();
            RestartClearTimer();
        }
    }

    void UpdateViolationText()
    {
        violationText.text = "Violations:\n";

        foreach (string v in ViolationManager.Instance.violations)
        {
            violationText.text += "- " + v + "\n";
        }
    }

    void RestartClearTimer()
    {
        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);

        clearCoroutine = StartCoroutine(ClearViolationTextAfterDelay());
    }

    IEnumerator ClearViolationTextAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        violationText.text = "";
        ViolationManager.Instance.violations.Clear();
        lastViolationCount = 0;
    }
}
