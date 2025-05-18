using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmount = 25;
    private ItemSpawner spawner;

    public void Initialize(ItemSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.AddHeath(healthAmount);

            if (spawner != null)
            {
                spawner.DecreaseItemCount();
            }

            Destroy(gameObject);
        }
    }
}
