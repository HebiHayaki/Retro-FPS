using NUnit.Framework;
using UnityEngine;

public class MockPlayerController : PlayerController
{
    public override bool IsFireButtonPressed()
    {
        return true; // Giả lập nút bắn luôn được nhấn
    }
}

[TestFixture]
public class PlayerControllerTests
{
    private GameObject playerObject;
    private PlayerController playerController;

    [SetUp]
    public void SetUp()
    {
        playerObject = new GameObject();
        playerController = playerObject.AddComponent<MockPlayerController>();

        // Mock dependencies
        playerController.theRB = playerObject.AddComponent<Rigidbody2D>();
        playerController.viewCam = playerObject.AddComponent<Camera>();
        playerController.deadScreen = new GameObject();
        playerController.healthTex = new GameObject().AddComponent<UnityEngine.UI.Text>();
        playerController.ammoTex = new GameObject().AddComponent<UnityEngine.UI.Text>();

        playerController.currentAmmo = 10;
        playerController.maxHealth = 100;
        playerController.currentHealth = 100;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(playerObject);
    }

    [Test]
    public void HandleMovement_WhenCalled_SetsVelocityCorrectly()
    {
        // Arrange
        playerController.moveSpeed = 5f;
        playerController.anim = playerObject.AddComponent<Animator>();
        Input.simulateMouseWithTouches = false;

        // Act
        playerController.HandleMovement();

        // Assert
        Assert.AreEqual(Vector2.zero, playerController.theRB.linearVelocity);
    }

    [Test]
    public void TakeDamage_WhenCalled_ReducesHealth()
    {
        // Arrange
        int initialHealth = playerController.currentHealth;
        int damageAmount = 20;

        // Act
        playerController.TakeDamage(damageAmount);

        // Assert
        Assert.AreEqual(initialHealth - damageAmount, playerController.currentHealth);
    }

    [Test]
    public void AddHealth_WhenCalled_IncreasesHealth()
    {
        // Arrange
        playerController.currentHealth = 50;
        int healAmount = 30;

        // Act
        playerController.AddHeath(healAmount);

        // Assert
        Assert.AreEqual(80, playerController.currentHealth);
    }

    [Test]
    public void AddHealth_WhenCalled_DoesNotExceedMaxHealth()
    {
        // Arrange
        playerController.currentHealth = 90;
        int healAmount = 20;

        // Act
        playerController.AddHeath(healAmount);

        // Assert
        Assert.AreEqual(playerController.maxHealth, playerController.currentHealth);
    }

    [Test]
    public void Die_WhenCalled_SetsHasDiedToTrue()
    {
        // Act
        playerController.Die();

        // Assert
        Assert.IsTrue(playerController.hasDied);
    }

    [Test]
    public void UpdateAmmoUI_WhenCalled_UpdatesAmmoText()
    {
        // Arrange
        playerController.currentAmmo = 5;

        // Act
        playerController.UpdateAmmoUI();

        // Assert
        Assert.AreEqual("5", playerController.ammoTex.text);
    }

    [Test]
    public void HandleShooting_WhenAmmoIsAvailable_DecreasesAmmo()
    {
        // Arrange
        playerController.currentAmmo = 5;
        playerController.fireCooldown = 0f;

        // Act
        playerController.HandleShooting();

        // Assert
        Assert.AreEqual(4, playerController.currentAmmo);
    }

    [Test]
    public void HandleShooting_WhenNoAmmo_DoesNotDecreaseAmmo()
    {
        // Arrange
        playerController.currentAmmo = 0;
        playerController.fireCooldown = 0f;

        // Act
        playerController.HandleShooting();

        // Assert
        Assert.AreEqual(0, playerController.currentAmmo);
    }
}
