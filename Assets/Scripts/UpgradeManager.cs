using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("Player Stats")]
    public float weaponDamage = 1.0f;
    public float fireRate = 0.5f;

    [Header("Upgrade UI")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeButton[] upgradeButtons;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowUpgradeOptions()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        PlayerController.instance.isCameraFrozen = true;

        upgradePanel.SetActive(true);

        upgradeButtons[0].Setup("Tăng sát thương", () => IncreaseWeaponDamage());
        upgradeButtons[1].Setup("Tăng tốc độ bắn", () => IncreaseFireRate());
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        PlayerController.instance.isCameraFrozen = false;

    }

    private void IncreaseWeaponDamage()
    {
        weaponDamage += 0.25f;
        CloseUpgradePanel();
    }

    private void IncreaseFireRate()
    {
        fireRate = Mathf.Max(0.1f, fireRate - 0.1f);
        CloseUpgradePanel();
    }
}
