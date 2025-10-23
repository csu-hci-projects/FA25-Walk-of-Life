using UnityEngine;

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
        if (!seesPlayer)
        {
            animator.SetBool("SeesPlayer", false);
            return;
        }

        Vector3 flatEnemyPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayerPos = new Vector3(player.position.x, 0, player.position.z);
        float distance = Vector3.Distance(flatEnemyPos, flatPlayerPos);

        animator.SetFloat("DistancePlayer", distance);
        animator.SetBool("SeesPlayer", true);

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
}
