using UnityEngine;
using UnityEngine.SceneManagement; 
public class DeathFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnTriggerEnter(Collider other)
    {
        
        if( other.gameObject.tag == "Player")
        {
            Debug.Log(other.gameObject.name + " entered the death floor");  
            SceneManager.LoadScene("DeathScreen");
        }
        else if( other.gameObject.tag == "Enemy")
        {
            Debug.Log(other.gameObject.name + "entered the death floor");  

        }
        else if( other.gameObject.tag == "Boss")
        {
            Debug.Log(other.gameObject.name + "entered the death floor");  
        }
    }
    

}
