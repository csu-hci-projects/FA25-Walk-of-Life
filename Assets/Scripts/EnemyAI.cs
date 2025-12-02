using UnityEngine;
using System.Collections;
using WalkOfLife.FinalCharacterController;
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f;
    public float rotationSpeed = 5f;  
    public float attackDistance = 2f;

    [Header("Sound Effects")]

    public AudioSource audioSource;
    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] hurtSounds;
    public AudioClip[] idleSounds;
    public AudioClip[] walkSounds;

    public float idleCooldown = 3f;
    public float idleTimer = 0f;

    public float footCooldown = 3f;
    public float footTimer = 0f;
    public float attackCooldown = 3f;
    public float attackTimer = 0f;

    [Header("Jumpscare Settings")]
    public Camera mainCamera;
    public float cameraZoomDuration = 0.5f; 
    public float cameraZoomFOV = 30f;       
    public AudioClip jumpscareSound;
    public float jumpscareVolume = 1f;
    private float originalFOV;
    private bool hasJumpscared = false;

    [HideInInspector]
    public bool seesPlayer = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; 
    }

    void Update()
    {
        idleTimer -= Time.deltaTime;
        footTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        if (!seesPlayer)
        {
            animator.SetBool("SeesPlayer", false);
            if(idleTimer <= 0f)
            {
                PlayRandomSound(idleSounds);
                idleTimer = idleCooldown;
            }
            return;
        }

        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(flatEnemyPos, flatPlayerPos);

        animator.SetFloat("DistancePlayer", distance);
        animator.SetBool("SeesPlayer", true);
        if (distance > stopDistance && footTimer <= 0f)
        {
            PlayRandomSound(walkSounds);
            footTimer = footCooldown;
        }
        if (distance <= attackDistance && attackTimer <= 0f)
        {
            PlayRandomSound(attackSounds);
            attackTimer = attackCooldown;
            if (!hasJumpscared && gameObject.CompareTag("Boss"))
            {
                hasJumpscared = true;
                StartCoroutine(BossJumpscareFirstPerson());
            }
        }
        if (distance > stopDistance)
        {
            Vector3 direction = (flatPlayerPos - flatEnemyPos).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            Vector3 moveVector = new Vector3(direction.x, 0, direction.z) * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + moveVector);
        }
    }

    // Called by the trigger
    public void PlayerDetected()
    {
        seesPlayer = true;
    }

    public void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            return;
        }
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
    IEnumerator BossJumpscareFirstPerson()
    {
        // 1. Freeze player movement
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.FreezeMovement();
        playerController.enabled = false;
        // 2. Get player's camera
        Camera playerCam = playerController.GetComponentInChildren<Camera>();
        if (playerCam == null) playerCam = Camera.main;

        // 3. Store original camera rotation
        Quaternion originalRotation = playerCam.transform.rotation;

        // 4. Compute direction to boss
        Vector3 directionToBoss = (transform.position - playerCam.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToBoss);

        // 5. Smoothly rotate camera toward boss over 0.5 seconds
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            playerCam.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            yield return null;
        }

        // 6. Play attack animation
        animator.SetTrigger("Attack");

        // 7. Play jumpscare sound
        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound, jumpscareVolume);

        // 8. Wait for the attack animation to finish
        float attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(attackDuration);

        // 9. Kill the player
        if (playerController != null)
            playerController.KillPlayer();
    }
}
