using UnityEngine;
using UnityEngine.UI;
public class QuickSlotUI :MonoBehaviour
{
    public Image leftHandWeaponIcon;
    public Image rightHandWeaponIcon;

    public void UpdateWeaponIcon(bool isleft, WeaponItem weapon)
    {
        if (isleft)
        {
            UpdateWeaponIcon(leftHandWeaponIcon, weapon);
        }
        else
        {
            UpdateWeaponIcon(rightHandWeaponIcon, weapon);
        }
    }

    private void UpdateWeaponIcon(Image icon, WeaponItem weapon)
    {
        if (weapon.itemIcon != null)
        {
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}


