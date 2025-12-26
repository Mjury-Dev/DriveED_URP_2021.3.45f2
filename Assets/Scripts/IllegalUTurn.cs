using UnityEngine;

public class IllegalUTurn : MonoBehaviour
{
    public float uTurnAngle = 150f; // angle threshold
    public int penalty = 20;

    private bool insideZone = false;
    private Vector3 entryDirection;
    private Transform car;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        car = other.transform;
        entryDirection = car.forward;
        insideZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!insideZone || !other.CompareTag("Player")) return;

        Vector3 exitDirection = car.forward;
        float angle = Vector3.Angle(entryDirection, exitDirection);

        if (angle >= uTurnAngle)
        {
            ViolationManager.Instance.AddViolation(
                "Illegal U-Turn", penalty);
        }

        insideZone = false;
    }
}