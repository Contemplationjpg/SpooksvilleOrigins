using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using UnityEngine;

public class InventorySerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        InventoryContainer[] inventory = (InventoryContainer[])obj;
        int j = 0;

        for (int i = 0; i < inventory.Length; i++) //breaking down item info
        {
            try 
            {
                info.AddValue("ItemName" + i, inventory[i].item.itemName);
            }
            catch
            {
                info.AddValue("numberOfUniqueItems", j);
                return;
            }
            info.AddValue("isDefaultItem" + i, inventory[i].item.isDefaultItem);
            info.AddValue("isStackable" + i, inventory[i].item.isStackable);
            info.AddValue("maxStackSize" + i, inventory[i].item.maxStackSize);
            info.AddValue("amount" + i, inventory[i].amount);
            j++;
        }
        info.AddValue("numberOfUniqueItems", j);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        List<InventoryContainer> inventory = (List<InventoryContainer>)obj;//new List<InventoryContainer>(); 
        //InventoryContainer[] inventory = (InventoryContainer[])obj;
        for(int i = 0; i < (int)info.GetValue("numberOfUniqueItems", typeof(int)); i++)
        {
            Item newItem = new Item
            {
                itemName = (string)info.GetValue("itemName" + i, typeof(string)),
                isDefaultItem = (bool)info.GetValue("isDefaultItem" + i, typeof(bool)),
                isStackable = (bool)info.GetValue("isStackable" + i, typeof(bool)),
                maxStackSize = (int)info.GetValue("itemName" + i, typeof(int))
            };

            InventoryContainer newInventoryContainer = new InventoryContainer(newItem, (int)info.GetValue("amount" + i, typeof(int)));
            inventory.Add(newInventoryContainer);
        }
        obj = inventory;
        return obj;
    }
}
