using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attractDistance = 5f;
    [SerializeField] private int expValue = 10;

    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    public void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attractDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ExpManager expManager = FindFirstObjectByType<ExpManager>();
            if (expManager != null)
            {
                expManager.AddExp(expValue);
            }

            Destroy(gameObject);
        }
    }
}
