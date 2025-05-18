using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 25;
    private ItemSpawner spawner;

    public void Initialize(ItemSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.currentAmmo += ammoAmount;
            PlayerController.instance.UpdateAmmoUI();

            if (spawner != null)
            {
                spawner.DecreaseItemCount();
            }

            Destroy(gameObject);
        }
    }
}
