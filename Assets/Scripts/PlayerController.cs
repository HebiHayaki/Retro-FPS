using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D theRB;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("View Control")]
    private Vector2 mouseInput;
    public float mouseSensitivity = 1f;
    public Camera viewCam;
    float maxAngle = 160;
    float minAngle = 10;

    [Header("Shooting")]
    public GameObject bulletImpact;
    public float fireCooldown = 0f;

    [Header("Player Stats")]
    public int currentAmmo;
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public GameObject deadScreen;
    public Text healthTex, ammoTex;

    [Header("Animation")]
    public Animator gunAnim;
    public Animator anim;

    public bool hasDied;
    public bool isCameraFrozen = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthTex.text = currentHealth.ToString() + "%";
        ammoTex.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied && !PauseMenu.isPaused)
        {
            HandleMovement();
            HandleViewControl();
            HandleShooting();
        }
    }

    public void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        theRB.linearVelocity = (moveHorizontal + moveVertical) * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void HandleViewControl()
    {
        if (isCameraFrozen) return;

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        Vector3 RotAmount = viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f);

        viewCam.transform.localRotation = Quaternion.Euler(RotAmount.x, Mathf.Clamp(RotAmount.y, minAngle, maxAngle), RotAmount.z);
    }

    public virtual bool IsFireButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    public void HandleShooting()
    {
        fireCooldown -= Time.deltaTime;

        if (IsFireButtonPressed() && fireCooldown <= 0f)
        {
            if (currentAmmo > 0)
            {
                Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Instantiate(bulletImpact, hit.point, transform.rotation);

                    if (hit.transform.tag == "Enemy")
                    {
                        hit.transform.parent.GetComponent<EnemyController>().TakeDamage(UpgradeManager.instance.weaponDamage);
                    }
                }
                else
                {
                    Debug.Log("I'm looking at nothing");
                }

                currentAmmo--;
                gunAnim.SetTrigger("Shoot");
                UpdateAmmoUI();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }

        healthTex.text = currentHealth.ToString() + "%";
    }

    public void AddHeath(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthTex.text = currentHealth.ToString() + "%";
    }

    public void UpdateAmmoUI()
    {
        ammoTex.text = currentAmmo.ToString();
    }

    public void Die()
    {
        deadScreen.SetActive(true);
            hasDied = true;
            currentHealth = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
    }
}
