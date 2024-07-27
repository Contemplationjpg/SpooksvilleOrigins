
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Collections.LowLevel.Unsafe;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;



[Serializable]
public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public event Action OnItemChangedCallBack;
    public bool UseSavedInventoryOnAwake = false;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }
        instance = this;

        currentInventorySpace = items.Count;

    }

    public int maxInventorySpace = 20;
    public int currentInventorySpace = 0;


   public List<InventoryContainer> items = new List<InventoryContainer>();

   public int AddItem(Item newItem, int addAmount) //changed from returning true or false based on if any item was added to returning the number of items total that were added
   {
    int amountAdded = 0;
    if (addAmount<0)
    {
        RemoveItem(newItem, -addAmount);
        return addAmount;
    }
    checkagain:
        if (newItem != newItem.isDefaultItem)
        {
            if (newItem.isStackable)
            {
                restackitems:
                for (int i = 0; i<currentInventorySpace; i++)
                {
                    if (items[i].item == newItem && items[i].amount<items[i].item.maxStackSize)
                    {
                        items[i].amount += addAmount; //ex. addAmount = 5, amount=0+5;
                        if (items[i].amount > items[i].item.maxStackSize) //ex. maxStackSize=3, amount:5 > mSS: 3
                        {
                            int diffInAddAmount = items[i].amount - items[i].item.maxStackSize; //dIAA = amount:5 - mSS:3 = 2
                            amountAdded += addAmount - diffInAddAmount; // amountAdded = amountAdded: 0 + addAmount:5 - dIAA:2 = 3  added 3 items so far
                            addAmount = diffInAddAmount; // addAmount:5 => dIAA:2   2 more items left to add
                            items[i].amount = items[i].item.maxStackSize; //cap amount at max stack size
                            goto checkagain; //run again but with new addAmount:2
                        }
                        Debug.Log("ADDING " + addAmount + " " + items[i].item.itemName);
                        amountAdded += addAmount;
                        OnItemChangedCallBack.Invoke();
                        return amountAdded;
                    }
                }
                if (currentInventorySpace >= maxInventorySpace+1)
                {
                    Debug.Log("Not enough inventory space");
                    OnItemChangedCallBack.Invoke();
                    return amountAdded;
                }
                if (addAmount > newItem.maxStackSize)
                {
                    items.Add(new InventoryContainer(newItem, 1));
                    addAmount -= 1;
                    amountAdded += 1;
                    currentInventorySpace++;
                    goto restackitems;
                }
                items.Add(new InventoryContainer(newItem, addAmount));
                amountAdded += addAmount;
                Debug.Log("ADDING NEW STACKABLE ITEM " + newItem.itemName);
                currentInventorySpace++;
                OnItemChangedCallBack.Invoke();
                return amountAdded;
            }
            else
            {
                if (currentInventorySpace >= maxInventorySpace)
                {
                    Debug.Log("Not enough inventory space");
                    OnItemChangedCallBack.Invoke();
                    return amountAdded;
                }
                items.Add(new InventoryContainer(newItem, 1));
                Debug.Log("ADDING NEW NON-STACKABLE ITEM " + newItem.itemName);
                currentInventorySpace++;
                addAmount -=1;
                amountAdded += 1;
                if (addAmount>=1)
                {
                    goto checkagain;
                }
                OnItemChangedCallBack.Invoke();
                return amountAdded;
            }
        }
        return amountAdded;
   }

    public void RemoveItem(Item item, int amount)
    {

        for (int i = currentInventorySpace-1; i > 0; i--)
        {
            if (items[i].item == item)
            {
                items[i].amount += amount;
                if (items[i].amount == 0)
                {
                    items.RemoveAt(i);
                    currentInventorySpace -= 1;
                }
                OnItemChangedCallBack.Invoke();
                return;
            }
        }
    }

    public void RemoveItem(int index, int amount)
    {
        InventoryContainer newItem = items[index];
        newItem.amount += amount;
        if (newItem.amount<=0)
        {
            items.RemoveAt(index);
            currentInventorySpace--;
            OnItemChangedCallBack.Invoke();
            return;
        }
        items[index] = newItem;
        OnItemChangedCallBack.Invoke();
    }

    public void ReorganizeInventory()
    {
        InventoryContainer[] tempItems = items.ToArray();
        for (int i = 0; i < tempItems.Length; i++)
        {
            if(tempItems[i]!=null)
            {
                if (tempItems[i].item.isStackable&&(tempItems[i].amount<tempItems[i].item.maxStackSize))
                {
                    InventoryContainer currentItem = tempItems[i];
                    Debug.Log("Currently Sorting Item: " + currentItem.item.itemName);
                    for(int j = i+1; j < tempItems.Length; j++)
                    {
                        if(tempItems[j]!=null)
                        {
                            InventoryContainer checkItem = tempItems[j];
                            Debug.Log("Checking Item, " + currentItem.item.itemName + ", Against Item: " + checkItem.item.itemName);
                            if(tempItems[j].item == tempItems[i].item)
                            {
                                int diffFromMaxStack = currentItem.item.maxStackSize - currentItem.amount;
                                int amountInCheckItem = checkItem.amount;
                                if (amountInCheckItem-diffFromMaxStack>=0)
                                {
                                    Debug.Log("Moving From Checked Item, " +  checkItem.item.itemName +", To Current Item: " + diffFromMaxStack.ToString());
                                    tempItems[i].amount += diffFromMaxStack;
                                    tempItems[j].amount -= diffFromMaxStack;
                                    Debug.Log("Current Item, " + currentItem.item.itemName + ", Amount: " + diffFromMaxStack.ToString());
                                    Debug.Log("Remaining Checked Item, " + checkItem.item.itemName + ", Amount: " + diffFromMaxStack.ToString());
                                }
                                else
                                {
                                    Debug.Log("Moving From Checked Item, " +  checkItem.item.itemName +", To Current Item: " + amountInCheckItem.ToString());
                                    tempItems[i].amount += amountInCheckItem;
                                    tempItems[j].amount -= amountInCheckItem;
                                    Debug.Log("Current Item, " + currentItem.item.itemName + ", Amount: " + diffFromMaxStack.ToString());
                                    Debug.Log("Remaining Checked Item, " + checkItem.item.itemName + ", Amount: " + diffFromMaxStack.ToString());
                                }

                                if (tempItems[j].amount == 0)
                                {
                                    Debug.Log("Nulling Item: " + checkItem.item.itemName + " In Slot: " + j);
                                    tempItems[j] = null;
                                }
                            }
                        }
                    }
                }   
            }
        }

        ResetInventory();
        for(int i = 0; i < tempItems.Length; i++)
        {
            if (tempItems[i]!=null)
            {
                AddItem(tempItems[i].item,tempItems[i].amount);
            }
            else
            {
                Debug.Log("Item In Slot " +  i +" Is Null");
            }
        }


    }

    public void LoadInventory(string[] itemNames, int[] itemAmounts)
    {
        ResetInventory();
        // Debug.Log(itemNames.Length);
        // if (itemNames.Length != 0)
        // Debug.Log(ItemSystem.instance.GetItem(itemNames[0]).itemName);

        if (itemNames.Length == 0)
        {
            // Debug.Log("Inventory rebuilt. (empty inventory)");
            OnItemChangedCallBack.Invoke();
            return;
        }

         for (int i = 0; i < itemNames.Length; i++)
        {
            // Debug.Log("RECREATING " + itemAmounts[i] + " " + ItemSystem.instance.GetItem(itemNames[i]).itemName);
            AddItem(ItemSystem.instance.GetItem(itemNames[i]),itemAmounts[i]);
        }
        // Debug.Log("Inventory rebuilt.");
        OnItemChangedCallBack.Invoke();
    }

    public void ResetInventory()
    {
        // Debug.Log("Emptying Inventory...");
        items = new List<InventoryContainer>();
        currentInventorySpace = 0;
    }


}

[Serializable]
public class InventoryContainer
{
    public Item item;
    public int amount;
    public InventoryContainer(Item newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
    }


}
