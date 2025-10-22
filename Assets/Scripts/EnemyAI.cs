using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Assign the player in inspector
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stopDistance = 1.5f; // Enemy stops moving at this distance
    public float rotationSpeed = 5f;  // How fast the enemy rotates toward player
    public float attackDistance = 2f; // Animator attack threshold

    [HideInInspector]
    public bool seesPlayer = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Gravity is on
    }

    void Update()
    {
        if (!seesPlayer)
        {
            animator.SetBool("SeesPlayer", false);
            return;
        }

        // Distance on XZ plane only
        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(flatEnemyPos, flatPlayerPos);

        // Update animator
        animator.SetFloat("DistancePlayer", distance);
        animator.SetBool("SeesPlayer", true);

        if (distance > stopDistance)
        {
            // Move toward player
            Vector3 direction = (flatPlayerPos - flatEnemyPos).normalized;

            // Smooth rotation toward player
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            // Move using Rigidbody
            Vector3 moveVector = new Vector3(direction.x, 0, direction.z) * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + moveVector);
        }
        // else: within stopDistance, enemy stops moving for attack

        // Animator will automatically handle walking vs attacking based on DistancePlayer
        // (DistancePlayer < attackDistance → attack animation, else walking)
    }

    // Called by the trigger
    public void PlayerDetected()
    {
        seesPlayer = true;
    }
}
