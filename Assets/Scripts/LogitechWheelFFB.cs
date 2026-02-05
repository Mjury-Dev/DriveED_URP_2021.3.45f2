using System.Runtime.InteropServices;
using UnityEngine;

public class LogitechWheelFFB : MonoBehaviour
{
    [DllImport("LogitechSteeringWheelEnginesWrapper")]
    private static extern bool LogiSteeringInitialize(bool ignoreXInput);

    [DllImport("LogitechSteeringWheelEnginesWrapper")]
    private static extern void LogiUpdate();

    [DllImport("LogitechSteeringWheelEnginesWrapper")]
    private static extern bool LogiIsConnected(int index);

    [DllImport("LogitechSteeringWheelEnginesWrapper")]
    private static extern bool LogiPlaySpringForce(int index, int offset, int saturation, int coefficient);

    [DllImport("LogitechSteeringWheelEnginesWrapper")]
    private static extern bool LogiPlayConstantForce(int index, int magnitude);

    void Start()
    {
        LogiSteeringInitialize(false);

        if (LogiIsConnected(0))
        {
            // Centering spring
            LogiPlaySpringForce(
                0,
                0,      // center offset
                50,     // saturation
                50      // strength
            );
        }
    }

    void Update()
    {
        LogiUpdate();
    }

    public void ApplyRoadForce(float force)
    {
        int magnitude = Mathf.Clamp((int)(force * 100), -100, 100);
        LogiPlayConstantForce(0, magnitude);
    }
}
