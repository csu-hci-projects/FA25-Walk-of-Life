using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public EnemyAI[] enemies; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (EnemyAI enemy in enemies)
            {
                if(enemy != null)
                {
                    enemy.PlayerDetected();
                }
            }
        }
    }
}
