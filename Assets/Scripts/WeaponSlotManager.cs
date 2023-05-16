using UnityEngine;

public class WeaponSlotManager :MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    Animator animator;
    QuickSlotUI quickSlotUI;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        quickSlotUI = FindObjectOfType<QuickSlotUI>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot) 
                leftHandSlot = weaponSlot;
            else if (weaponSlot.isRightHandSlot)
                rightHandSlot = weaponSlot;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            quickSlotUI.UpdateWeaponIcon(true, weaponItem);
            LoadLeftWeaponDamageCollider();

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("LeftArmEmpty", 0.2f);
            }    
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            quickSlotUI.UpdateWeaponIcon(false, weaponItem);
            LoadRightWeaponDamageCollider();

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
            }
            else
            {
                animator.CrossFade("RightArmEmpty", 0.2f);
            }
        }
    }

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponent<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponent<DamageCollider>();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }
}