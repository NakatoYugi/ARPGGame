using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
        PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        playerAnimatorManager animatorHandler = playerManager.GetComponentInChildren<playerAnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        playerInventory.weaponsInventory.Add(weapon);
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);

        playerManager.interactableItemGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.interactableItemGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.interactableItemGameObject.SetActive(true);

        Destroy(gameObject);
    }
}


