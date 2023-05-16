using UnityEngine;

public class UIHandler :MonoBehaviour
{
    public PlayerInventory playerInventory;

    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject SelectWindow;
    public GameObject WeaponInventoryWindow;

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerInventory.weaponsInventory.Count)
            {
                if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }
        #endregion
    }

    public void OpenSelectWindow()
    {
        SelectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        SelectWindow.SetActive(false);
    }

    public void CloseAllWeaponInventoryWindow()
    {
        WeaponInventoryWindow.SetActive(false);
    }
}


