using UnityEngine;

public class SlimeHitboxEvents : MonoBehaviour
{
    [SerializeField] private HitboxManager SlimeHitbox;

    public void EnableSlimeHitbox()
    {
        if (SlimeHitbox != null)
            SlimeHitbox.EnableHitbox();
    }

    public void DisableSlimeHitbox()
    {
        if (SlimeHitbox != null)
            SlimeHitbox.DisableHitbox();
    }
}
