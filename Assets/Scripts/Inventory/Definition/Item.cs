using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string displayName = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public bool isStackable = false;
    public int maxStackSize = 99;


    public virtual void Use()
    {
        Debug.Log("Using " + itemName);
    }


}
