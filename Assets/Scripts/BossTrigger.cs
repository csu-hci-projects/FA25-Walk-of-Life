using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public EnemyAI[] enemies; 
    public ScriptedBehavior bossScript;

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
            if (bossScript != null)
            {
                bossScript.StartChase();
            }
        }
    }
}
