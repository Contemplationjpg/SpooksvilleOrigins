using System;
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
    }
    public void Start() 
    {
        CloseDisplay();
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

     public void SetLootSlot(int slot, String perk)
     {
        if (slot < lootSlots.Length)
        {
            lootSlots[slot].SetPerk(perk);
        }
     }

    public void SetLootAsTestItem(int slot)
    {
        SetLootSlot(slot, testItem, 1);
    }

    public void SetAllLoot(EncounterType enc)
    {
        for (int i = 0; i < enc.loot.Length; i++) 
        { 
            lootSlots[i].ClearLootSlot();
            if (enc.loot[i]!=null)
            {
                // print("setting loot slot " + i + " to item: " + enc.loot[i].itemName);
                SetLootSlot(i, enc.loot[i], enc.lootCounts[i]);
            }
            else if (!enc.perkLoot.Equals(""))
            {
                SetLootSlot(i, enc.perkLoot[i]);
            }
        }
    }

    public void StartLooting()
    {
        SetAllLoot(BattleManager.instance.currentEncounter);
        BattleManager.instance.increaseEncounterNumber();
        OpenDisplay();
    }


}
