using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public EnemyAI boss; // Drag your boss/enemy here in inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.PlayerDetected();
            // Optional: disable trigger so it only fires once
            // gameObject.SetActive(false);
        }
    }
}
