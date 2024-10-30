using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class LootManager : MonoBehaviour
{
    LootManager instance;

    [SerializeField]
    private GameObject lootPanel;
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






}
