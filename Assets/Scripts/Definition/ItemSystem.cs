using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemSystem : ScriptableObject
{
    public Item[] itemDatabase;
    public static ItemSystem instance;

    public void RefreshDatabase()
    {
        instance = this;
    }

    public Item GetItem(int itemID)
    {   
        Item getItem = CreateInstance<Item>();
        getItem.isDefaultItem = true;

        if (itemID < itemDatabase.Length)
        {
            getItem = itemDatabase[itemID];
        }
        
        return getItem;
    }

    public Item GetItem(string ItemName)
    {
        Item getItem = CreateInstance<Item>();
        getItem.isDefaultItem = true;
        
        for (int i = 0; i < itemDatabase.Length;i++)
        {
            if (ItemName == itemDatabase[i].itemName)
            {
                getItem = itemDatabase[i];
                return getItem;
            }
        }

        return getItem;
    }

    public int GetItemID(Item item)
    {
        int searchItemID = 0;

        for(int i = 0; i < itemDatabase.Length-1;i++)
        {
            if (itemDatabase[i].itemName == item.itemName)
            {
                searchItemID = i;
                Debug.Log("ID for " + item.itemName + " is " + searchItemID);
                return searchItemID;
            }
        }
        return searchItemID;
    }

}
