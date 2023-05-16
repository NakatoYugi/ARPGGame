using UnityEngine;

public class TakeDamage :MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(25);
        }
    }
}


