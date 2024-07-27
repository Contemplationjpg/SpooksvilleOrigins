using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponDatabase", menuName = "Inventory/WeaponDatabase")]
public class WeaponSystem : ScriptableObject
{
    public Weapon[] weaponDatabase;
    public static WeaponSystem instance;

    public void RefreshDatabase()
    {
        instance = this;
    }

    public Weapon GetWeapon(int weaponID)
    {   
        Weapon getWeapon = CreateInstance<Weapon>();
        getWeapon.isDefaultItem = true;

        if (weaponID < weaponDatabase.Length)
        {
            getWeapon = weaponDatabase[weaponID];
        }
        
        return getWeapon;
    }

    public Weapon GetWeapon(string weaponName)
    {
        Weapon getWeapon = CreateInstance<Weapon>();
        getWeapon.isDefaultItem = true;
        
        for (int i = 0; i < weaponDatabase.Length;i++)
        {
            if (weaponName == weaponDatabase[i].itemName)
            {
                getWeapon = weaponDatabase[i];
                return getWeapon;
            }
        }

        return getWeapon;
    }

    public int GetWeaponID(Weapon weapon)
    {
        int searchWeaponID = 0;

        for(int i = 0; i < weaponDatabase.Length-1;i++)
        {
            if (weaponDatabase[i].itemName == weapon.itemName)
            {
                searchWeaponID = i;
                Debug.Log("ID for " + weapon.itemName + " is " + searchWeaponID);
                return searchWeaponID;
            }
        }
        return searchWeaponID;
    }

}
