using System;
using UnityEngine;


public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory instance;
    public int maxInventorySpace = 4;
    public int currentInventorySpace = 0;
    public WeaponInventoryContainer[] weapons;
    public event Action OnWeaponChangedCallBack;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }
        weapons = new WeaponInventoryContainer[maxInventorySpace];
        instance = this;
    }

    public bool AddWeapon(Weapon weapon, int durability)
    {
        if (currentInventorySpace < maxInventorySpace)
        {
            if (!weapon.isDefaultItem)
            {
                WeaponInventoryContainer newWeaponInventoryContainer = new WeaponInventoryContainer(weapon, durability);
                weapons[currentInventorySpace] = newWeaponInventoryContainer;
                currentInventorySpace += 1;
                Debug.Log("Added Item: " + weapon.itemName);
                OnWeaponChangedCallBack.Invoke();
                return true;
            }
            return false;
        }
        return false;
    }

    public void RemoveWeapon(int weaponSlot)
    {
        if (weaponSlot <= weapons.Length-1)
        {
            weapons[weaponSlot] = null;
            currentInventorySpace -=1;
            OnWeaponChangedCallBack.Invoke();
        }
        
    }

    public void ReduceDurability(int weaponSlot, int reduceDurability)
    {
        if (weaponSlot <= weapons.Length-1)
        {
            weapons[weaponSlot].durability -= reduceDurability;
            if (weapons[weaponSlot].durability<0)
            {
                weapons[weaponSlot].durability = 0;
            }
        }
        OnWeaponChangedCallBack.Invoke(); 
    }

    public void IncreaseDurability(int weaponSlot, int increaseDurability)
    {
        if (weaponSlot <= weapons.Length-1)
        {
            weapons[weaponSlot].durability += increaseDurability;
            if (weapons[weaponSlot].durability>0)
            {
                weapons[weaponSlot].durability = weapons[weaponSlot].weapon.maxDurability;
            }
        } 
        OnWeaponChangedCallBack.Invoke();
    }

    public int GetDurability(int weaponSlot)
    {
        if (weapons[weaponSlot]!=null)
        {
            return weapons[weaponSlot].durability;
        }
        else
        return 0;
    }

    public void LoadWeaponInventory(string weaponNames, int[] weaponDurabilities)
    {
        ResetWeaponInventory();
        for (int i = 0; i < weaponNames.Length; i++)
        {
            AddWeapon(WeaponSystem.instance.GetWeapon(weaponNames[i]), weaponDurabilities[i]);
        }
        OnWeaponChangedCallBack.Invoke();
    }

    public void ResetWeaponInventory()
    {
        weapons = new WeaponInventoryContainer[maxInventorySpace];
        currentInventorySpace = 0;
        OnWeaponChangedCallBack.Invoke();
    }





}

[Serializable]
public class WeaponInventoryContainer
{
    public Weapon weapon;
    public int durability;
    public WeaponInventoryContainer(Weapon newWeapon, int newDurability)
    {
        weapon = newWeapon;
        durability = newDurability;
    }


}