using System.Collections;
using System.Collections.Generic;
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

    public void SetLootAsTestItem(int slot)
    {
        SetLootSlot(slot, testItem, 1);
    }




}
