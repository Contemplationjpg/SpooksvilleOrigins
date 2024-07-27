using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TMP_Text amountNumber;

    Item item;
    int itemIndex;
    private void Awake()
    {
        icon.enabled = false;
        amountNumber.text = "";
    }

    public void AddItem(Item newItem, int amount, int index)
    {
        item = newItem;
        itemIndex = index;

                
        icon.sprite = item.icon;
        icon.enabled = true;
        if (!item.isStackable)
        {
            amountNumber.text = "";
        }
        else
        amountNumber.text = amount.ToString();
        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        amountNumber.text = "";
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Debug.Log("Removing 1 " + item.itemName);
        Inventory.instance.RemoveItem(itemIndex, -1);
    }

    public void UseItem()
    {
        if (item!=null)
        {
            item.Use();
        }
    }

}
