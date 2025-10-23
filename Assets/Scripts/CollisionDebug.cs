using UnityEngine;

public class CollisionDebug : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} triggered with {other.gameObject.name}");
    }
}
