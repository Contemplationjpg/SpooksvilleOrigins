using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
   public int maxHealth;
   public int currentHealth;
   public float[] position;
   public string[] ItemNames;
   public int[] ItemCounts;
   public string[] WeaponNames;
   public int[] WeaponDurabilities;
   public PlayerData (Player player)
   {
      maxHealth = player.maxHealth;
      currentHealth = player.currentHealth;

      position = new float[2];
      position[0] = player.transform.position.x;
      position[1] = player.transform.position.y;


      if (Inventory.instance.items.Count != 0)
      {
         ItemNames = new string[Inventory.instance.items.Count];
         ItemCounts = new int[Inventory.instance.items.Count];
         for (int i = 0; i < Inventory.instance.items.Count; i++)
         {
            ItemNames[i] = Inventory.instance.items[i].item.itemName;
            ItemCounts[i] = Inventory.instance.items[i].amount;
            Debug.Log("Added item to playerdata: " + ItemCounts[i] + " " + ItemNames[i]);
         }  
      }
      else
      {
         ItemNames = new string[0];
         ItemCounts = new int[0];
      }

      if (WeaponInventory.instance.currentInventorySpace !=0)
      {
         WeaponNames = new string[WeaponInventory.instance.weapons.Length];
         WeaponDurabilities = new int[WeaponInventory.instance.weapons.Length];
         for (int i = 0; i < WeaponInventory.instance.weapons.Length; i++)
         {
            WeaponNames[i] = WeaponInventory.instance.weapons[i].weapon.itemName;
            WeaponDurabilities[i] = WeaponInventory.instance.weapons[i].durability;
            Debug.Log("Added weapon to playerdata: " + WeaponDurabilities[i] + " " + WeaponNames[i]);
         }  
      }
      else
      {
         WeaponNames = new string[0];
         WeaponDurabilities = new int[0];
      }
      
   }

}
