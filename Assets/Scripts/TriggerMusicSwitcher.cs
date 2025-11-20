using UnityEngine;

public class TriggerMusicSwitcher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource newMusic;
    public AudioSource backgroundNoise;

    private bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;

        if (other.CompareTag("Player"))
        {
            if (backgroundNoise != null)
            {
                backgroundNoise.Stop();
            }
            if(newMusic != null )
            {
                newMusic.Play();
            }
        }
    }
}
