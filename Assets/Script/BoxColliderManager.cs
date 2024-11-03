using UnityEngine;

public class BoxColliderManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            otherRigidbody.velocity = Vector3.zero;
            otherRigidbody.angularVelocity = Vector3.zero;
        }
    }
}
