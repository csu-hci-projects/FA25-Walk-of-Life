using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject key;
    public string enemyType; // this will be the slime color IE Green or Red
    void Start()
    {
        key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("number of " + enemyType +" slimes alive: "+ numberOfEnemies(enemyType));
        if (key != null){
            if(!key.activeSelf && numberOfEnemies(enemyType) <= 0)
            {
                key.SetActive(true);
                Debug.Log(enemyType + " key spawned");
                
            }
        }
    }
    
    //searches the program for all gameobjects with the tag Enemy then looks at their names for the
    // specific enemy type IE green or red
    private int numberOfEnemies(string enemyToSearchFor)
    {
        int count = 0;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        
        foreach (GameObject obj in enemies){
            if (obj.name.Contains(enemyToSearchFor))
            {
                count++;
            }
        }
        return count;
        
    }
}
