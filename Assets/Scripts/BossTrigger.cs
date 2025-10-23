using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public EnemyAI boss; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.PlayerDetected();
        }
    }
}
