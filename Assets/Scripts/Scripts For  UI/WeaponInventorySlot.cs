using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot :MonoBehaviour
{
    public Image icon;
    WeaponItem item;

    public void AddItem(WeaponItem newItem)
    {
        icon.sprite = newItem.itemIcon;
        item = newItem;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }
}


