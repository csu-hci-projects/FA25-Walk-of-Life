using UnityEngine;

public class PlayerHitboxEvents : MonoBehaviour
{
    [SerializeField] private HitboxManager rightHandHitbox;
    [SerializeField] private HitboxManager leftHandHitbox;

    public void EnableRightHitbox()
    {
        if (rightHandHitbox != null)
            rightHandHitbox.EnableHitbox();
    }

    public void DisableRightHitbox()
    {
        if (rightHandHitbox != null)
            rightHandHitbox.DisableHitbox();
    }

    public void EnableLeftHitbox()
    {
        if (leftHandHitbox != null)
            leftHandHitbox.EnableHitbox();
    }

    public void DisableLeftHitbox()
    {
        if (leftHandHitbox != null)
            leftHandHitbox.DisableHitbox();
    }
}
