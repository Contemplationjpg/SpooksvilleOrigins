using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Scripting;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    public Item testItem;

    [SerializeField]
    private GameObject lootPanel;
    [SerializeField]
    private LootSlot[] lootSlots;

    public void Awake() 
    {
        instance = this;
        CloseDisplay();
    }

    public void EncounterLoot() 
    {
        OpenDisplay();
        SetAllAsTestLoot(); //debug
    }

    private void SetAllAsTestLoot() //debug
    {
        for (int i = 0; i < lootSlots.Length;i++)
        {
            SetLootSlot(i, BattleManager.instance.weaponDatabase.GetWeapon(2),1);
        }
    }

    public void OpenDisplay()
    {
        lootPanel.SetActive(true);
    }

    public void CloseDisplay()
    {
        lootPanel.SetActive(false);
    }

    public void SetLootSlot(int slot, Item item, int amount)
    {
        if (slot < lootSlots.Length)
        {
            lootSlots[slot].SetItem(item, amount);
        }
    } 

    public void SetLootAsTestItem(int slot) //debug
    {
        SetLootSlot(slot, testItem, 1);
    }

    public void SetAllLoot(EncounterType enc)
    {
        for (int i = 0; i < enc.loot.Length; i++) 
        { 
            print("setting loot slot " + i + " to item: " + enc.loot[i].itemName);
            SetLootSlot(i, enc.loot[i], enc.lootCounts[i]);
        }
    }

    public void StartLooting()
    {
        SetAllLoot(BattleManager.instance.currentEncounter);
        BattleManager.instance.increaseEncounterNumber();
        OpenDisplay();
    }


}
