using UnityEngine;
public class PlayerStats : CharacterStats
{
    public HealthBar healthBar;
    playerAnimatorManager animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<playerAnimatorManager>();
    }


    private void Start()
    {
        maxHealth = SetMaxHealthFormHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFormHealthLevel()
    {
        int _maxHealth = healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation("Damage_01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Death_01", true);
        }
    }
}