using UnityEngine;

public class CollisionViolation : MonoBehaviour
{
    public int penalty = 15;
    public float minImpactSpeed = 2f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed < minImpactSpeed)
            return; // ignore small bumps

        ViolationManager.Instance.AddViolation("Collision", penalty);

        Debug.Log("Collision with: " + collision.gameObject.name);
    }
}
