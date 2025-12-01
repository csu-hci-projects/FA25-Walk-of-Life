using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxManager : MonoBehaviour
{
    private Collider hitbox;

     private void Awake()
    {
        hitbox = GetComponent<Collider>();
        hitbox.enabled = false; 
    }

    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }
}
