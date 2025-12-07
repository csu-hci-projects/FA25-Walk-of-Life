using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class ScriptedBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Vector3 pos1;
    [SerializeField] Vector3 pos2;
    [SerializeField] Vector3 pos3;

    [SerializeField] float speed;

    [SerializeField] private float stopDistance = 0.2f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private GameObject bloodExplosionPrefab;
    [SerializeField] private float destroyDelay = 0.1f;

    [SerializeField] private Transform player;

    private bool playerInRange = false;

    private Collider enemyCollider;

    private bool chaseActive = false;
    private int currentTarget = 0;
    private Vector3[] points;
    void Start()
    {
        points = new Vector3[] { pos1, pos2, pos3 };
        enemyCollider = GetComponent<Collider>();

        if (enemyCollider != null)
        {
            enemyCollider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!chaseActive) return;

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
            {
                playerInRange = true;
                speed = 0;
            }
            else
            {
                playerInRange = false;
            }
        }

        if (!playerInRange)
            Move();
    }

    public void StartChase()
    {
        chaseActive = true;
        currentTarget = 0;
    }

    private void Move()
    {
        if (currentTarget >= points.Length)
        {
            TriggerDeath();
            return;
        }

        Vector3 target = points[currentTarget];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 5f
            );
        }

        if (Vector3.Distance(transform.position, target) < stopDistance)
        {
            currentTarget++;
        }
    }
    
    private void TriggerDeath()
    {
        chaseActive = false;

        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
            ai.enabled = false;
        
        audioSource.enabled = true;

        if (bloodExplosionPrefab != null)
        {
            GameObject blood = Instantiate(bloodExplosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
        }
        Destroy(gameObject, destroyDelay);
    }
}
