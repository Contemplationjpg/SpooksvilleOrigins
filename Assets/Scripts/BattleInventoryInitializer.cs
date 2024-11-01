using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleInventoryManager : MonoBehaviour
{
public GameObject ItemSlot;
public GameObject ItemPanel;
private static bool BattleInventoryInitialized = false;
Inventory inventory;


void Start()
{
    inventory = Inventory.instance;
    inventory.OnItemChangedCallBack += UpdateUI;
    
}
public void InitializeBattleInventory()
{
    if (!BattleInventoryInitialized)
    {
        ResetInventory();
        CopyOverBattleInventory();
        BattleInventoryInitialized = true;
    }
}
public void CopyOverBattleInventory()
{
    foreach (InventoryContainer i in Inventory.instance.items)
    {
        // Instantiate(ItemSlot, new Vector2(0,0), quaternion.identity);
        string itemName = i.item.displayName;
        int itemCount = i.amount;

        var newSlot = Instantiate(ItemSlot,ItemPanel.transform);
        TMP_Text[] textBoxes = newSlot.GetComponentsInChildren<TMP_Text>();
        Button button = newSlot.GetComponent<Button>();
        
        button.onClick.AddListener(i.item.Use);
        textBoxes[0].text = itemName;
        textBoxes[1].text = itemCount.ToString();
    }
}

public void ResetInventory()
{
    foreach(Transform child in ItemPanel.transform)
    {
        Destroy(child.gameObject);
    }
}

void UpdateUI()
    {
        Debug.Log("Updating UI");
        if (SceneManager.GetActiveScene().name == "Battle")
        {
            ResetInventory();
            CopyOverBattleInventory();
        }
    }


}
