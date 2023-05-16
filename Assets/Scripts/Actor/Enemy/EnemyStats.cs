using UnityEngine;
public class EnemyStats : CharacterStats
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        maxHealth = SetMaxHealthFormHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFormHealthLevel()
    {
        int _maxHealth = healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Death_01");
        }
    }
}

