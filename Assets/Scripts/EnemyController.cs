using System.Net.NetworkInformation;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Health Settings")]
    public float health = 4f;

    [Header("Movement Settings")]
    public float playerRange = 50f;
    public float minDistance = 3f;
    public float maxDistance = 5f;
    public float moveSpeed;

    [Header("Shooting Settings")]
    public bool shouldShoot;
    public float fireRate = 1f;
    public GameObject bullet;
    public Transform firePoint;

    [Header("References")]
    public Rigidbody2D theRB;
    public GameObject explosion;
    [SerializeField]
    private GameObject expPrefab;

    private float shotCounter;
    private EnemySpawner spawner;
    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = PlayerController.instance.transform;
        shotCounter = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        Vector3 playerDirection = playerTransform.position - transform.position;
        float distanceToPlayer = playerDirection.magnitude;

        HandleMovement(playerDirection, distanceToPlayer);

        if (shouldShoot && distanceToPlayer < playerRange)
        {
            HandleShooting();
        }
    }

    private void HandleMovement(Vector3 playerDirection, float distanceToPlayer)
    {
        if (distanceToPlayer < playerRange)
        {
            if (distanceToPlayer < minDistance)
            {
                theRB.linearVelocity = -playerDirection.normalized * moveSpeed;
            }
            else if (distanceToPlayer > maxDistance)
            {
                theRB.linearVelocity = playerDirection.normalized * moveSpeed;
            }
            else
            {
                theRB.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            theRB.linearVelocity = Vector2.zero;
        }
    }

    private void HandleShooting()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            shotCounter = fireRate;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Initialize(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.DecreaseEnemyCount();
        }
    }
}
