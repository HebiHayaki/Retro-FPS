using UnityEngine;
using UnityEngine.Timeline;

public class EnemyBullet : MonoBehaviour
{
    public int damageAmount;

    public float bulletSpeed = 3f;

    public Rigidbody2D theRB;

    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        direction = direction * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        theRB.linearVelocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
