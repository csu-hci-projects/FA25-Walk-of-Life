using Unity.VisualScripting;
using UnityEngine;

public class CombatCollision : MonoBehaviour
{
    [SerializeField] private AttributesManager attacker;
    [SerializeField] private string targetTag;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;
        if (attacker == null)
        {
            Debug.LogWarning("Damage has now attacker set");
        }
        attacker.DealDamage(other.gameObject);
        Debug.Log("damage done to " + targetTag);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
