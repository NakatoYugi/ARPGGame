﻿using UnityEngine;

public class Interactable :MonoBehaviour
{
    public float radius = 0.6f;
    public string interactableText;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact(PlayerManager playerManager) 
    {
        Debug.Log("Picking Up Item");
    }
}


