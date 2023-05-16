using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory :MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];

    public List<WeaponItem> weaponsInventory;

    int rightHandSlotsCurrentIdx = 0;
    int leftHandSlotsCurrentIdx = 0;
    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        rightWeapon = weaponsInLeftHandSlots[rightHandSlotsCurrentIdx];
        leftWeapon = weaponsInRightHandSlots[leftHandSlotsCurrentIdx];

        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangRightHandWeapon()
    {
        rightWeapon = ChangeWeapon(weaponsInRightHandSlots,ref rightHandSlotsCurrentIdx);
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
    }

    public void ChangeLeftHandWeapon()
    {
        leftWeapon = ChangeWeapon(weaponsInLeftHandSlots, ref leftHandSlotsCurrentIdx);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    private WeaponItem ChangeWeapon(WeaponItem[] weaponHolderSlots, ref int idx)
    {
        idx += 1;
        idx %= weaponHolderSlots.Length;

        while (weaponHolderSlots[idx] == null)
        {
            idx += 1;
            idx %= weaponHolderSlots.Length;
        }
        
        return weaponHolderSlots[idx];
    }
}


