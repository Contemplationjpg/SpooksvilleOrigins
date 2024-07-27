using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangedCallBack += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        Debug.Log("Updating UI");
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            int j = 0;
            for (int i = 0; i < slots.Length; i++)
            {
            if (i < inventory.items.Count&&inventory.items[j].item!=null)
            {
                slots[i].AddItem(inventory.items[j].item, inventory.items[j].amount,j);
                j++;
            }
            else
            {
                slots[i].ClearSlot();
            }
            }
        }
    }
}
