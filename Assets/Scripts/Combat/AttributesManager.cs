using UnityEngine;
using UnityEngine.SceneManagement; 


public class AttributesManager : MonoBehaviour
{
    [SerializeField] public int health;
    [SerializeField] public int attack;

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            if( this.gameObject.tag == "Player")
            {
            Debug.Log("Player Died");  
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

        }
    }
    private void Die()
    {
        Debug.Log($"{gameObject.name} is dead.");
        Destroy(gameObject);
    }
}
