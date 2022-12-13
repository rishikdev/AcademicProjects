using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerHealthBar playerHealthBar;
    [SerializeField] private ShootingBuildingInteraction shootingBuilding;
    [SerializeField] private PlayerFire playerFire;
    [SerializeField] private PlayerMovement playerMovement;

    public float maxHealth = 500.0f;
    public float currentHealth = 500.0f;

    public void TakeDamage(int damageAmount)
    {
        currentHealth = Mathf.Max(0.0f, currentHealth - damageAmount);
        playerHealthBar.SetHealthBarPercentage(currentHealth / maxHealth);

        if (currentHealth <= 0.0f)
        {
            playerMovement.enabled = false;
            playerFire.enabled = false;
            shootingBuilding.isPlayerDead = true;
            currentHealth = maxHealth;
            playerHealthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        }
    }
}
