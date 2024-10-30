using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;
    private TMP_Text itemName;
    private TMP_Text itemDesc;
    private Item slotItem;
    private int itemAmount = 0;

    public void Awake()
    {
        slotItem = null;
    }
    public void SetItem(Item item, int amount)
    {
        slotItem = item;
        itemAmount = amount;
        itemIcon.sprite = item.icon;
        itemName.text = item.displayName;
        itemDesc.text = item.description;
    }

    public bool GiveItem()
    {
        try 
        {
            if (slotItem != null)
            {
                Inventory.instance.AddItem(slotItem, itemAmount);
                return true;
            }
            print("Null Item in LootSlot, could not give Item");
            return false;
        }
        catch (Exception ex)
        {
            print("could not give item in LootSlot");
            return false;
        }
    }



}
