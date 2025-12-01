using UnityEngine;

public class PunchSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip punchSFX;

    public void PlayPunchSound()
    {
        if (audioSource != null && punchSFX != null)
        {
            audioSource.PlayOneShot(punchSFX);
        }
    }
}