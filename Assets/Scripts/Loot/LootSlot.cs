using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public Item slotItem;
    public int itemAmount = 0;

    public void Awake()
    {
        // slotItem = null;
    }
    public void SetItem(Item item, int amount)
    {
        // print("setting item to: " + item.itemName);
        slotItem = item;
        itemAmount = amount;
        itemIcon.sprite = item.icon;
        itemName.text = item.displayName;
        itemDesc.text = item.description;
    }
    public void GiveLoot()
    {
        GiveItem();
    }
    public bool GiveItem()
    {
        try 
        {
            if (slotItem != null)
            {
                if (slotItem.GetType() == typeof(Weapon))
                {
                    WeaponInventory.instance.AddWeapon((Weapon)slotItem, WeaponSystem.instance.GetWeapon(slotItem.itemName).maxDurability);
                }
                else
                {
                Inventory.instance.AddItem(slotItem, itemAmount);    
                }
                return true;
            }
            print("Null Item in LootSlot, could not give Item");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            Debug.LogWarning("could not give item in LootSlot");
            return false;
        }
    }

    public void EndLooting() 
    {
        if (GiveItem())
        {
            LootManager.instance.CloseDisplay();
            if (!BattleManager.instance.SpawnEncounter()) 
            {
                TurnManager.instance.ChangePlayerActionable(false);
                TurnManager.instance.ChangeState(TurnManager.State.Win);
            }
            else
            {
                TurnManager.instance.player.ResetActionCount();
                TurnManager.instance.ChangePlayerActionable(true);
                TurnManager.instance.ChangeState(TurnManager.State.WaitingForPlayerInput);
            }
            
        }
    }



}
