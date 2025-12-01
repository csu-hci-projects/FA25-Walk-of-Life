using UnityEngine;
using UnityEngine.SceneManagement; 


public class AttributesManager : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public int health;
    [SerializeField] public int attack;

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            if( this.gameObject.tag == "Player")
            {
            SceneManager.LoadScene("DeathScreen");
            }
            else
            {
                Die();
            }
        }
    }
    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<AttributesManager>();
        if(atm != null)
        {
            atm.TakeDamage(attack);
            if (this.CompareTag("Player"))
        {
            var punchSound = GetComponent<PunchSoundManager>();
            if (punchSound != null)
            {
                punchSound.PlayPunchSound();
            }
        }
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
