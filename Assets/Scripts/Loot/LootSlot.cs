using System;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LootSlot : MonoBehaviour
{
    public bool isClear = true;
    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public Item slotItem;
    public int itemAmount = 0;
    public bool isPerk = false;
    public String perkName;

    public void Awake()
    {
        // slotItem = null;
    }


    void UpdateVisibility()
    {
        if (isClear)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    public void SetItem(Item item, int amount)
    {
        print("setting loot item to: " + item.itemName);
        isClear = false;
        isPerk = false;
        perkName = "";
        slotItem = item;
        itemAmount = amount;
        itemIcon.sprite = item.icon;
        itemName.text = item.displayName;
        itemDesc.text = item.description;
        UpdateVisibility();
    }
    public void SetPerk(String perk)
    {
        Perk perkObject;
        try 
        {
            perkObject = PerkSystem.instance.GetPerk(perk);
            Debug.Log("found perk: " + perkObject.perkName);
        }
        catch
        {
            ClearLootSlot();
            return;
        }
        
        if (!perkObject.perkName.Equals("default name"))
        {
            print("setting loot perk to: " + perkObject.perkName);
            isClear = false;
            isPerk = true;
            perkName = perk;
            itemIcon.sprite = perkObject.sprite;
            itemName.text = perkObject.displayName;
            itemDesc.text = perkObject.description;
            UpdateVisibility();

        }
        else
        {
            UpdateVisibility();
        }
    }
    public void ClearLootSlot()
    {
        isClear = true;
        itemIcon.sprite = null;
        itemName.text = "";
        itemDesc.text = "";
        slotItem = null;
        itemAmount = 0;
        isPerk = false;
        perkName = null;
        UpdateVisibility();
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
                ClearLootSlot();
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
        if (!isPerk)
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
        else
        {
            if (PerkManager.instance.GivePerk(perkName))
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



}
