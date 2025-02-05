using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "New PerkDatabase", menuName = "Perk/PerkDatabase")]
public class PerkSystem : ScriptableObject
{
    public Perk[] perkDatabase;
    public static PerkSystem instance;

    public void RefreshDatabase()
    {
        instance = this;
    }

     public Perk GetPerk(int perkID)
    {   
        Perk getPerk = CreateInstance<Perk>();

        if (perkID < perkDatabase.Length)
        {
            getPerk = perkDatabase[perkID];
            Debug.Log("perk at ID " + perkID + " is " + getPerk.perkName);
        }
        
        return getPerk;
    }

    public Perk GetPerk(string perkName)
    {
        Perk getPerk = CreateInstance<Perk>();
        
        for (int i = 0; i < perkDatabase.Length;i++)
        {
            if (perkName == perkDatabase[i].perkName)
            {
                getPerk = perkDatabase[i];
                return getPerk;
            }
        }

        return getPerk;
    }

    public int GetPerkID(Perk perk)
    {
        int searchPerkID = 0;

        for(int i = 0; i < perkDatabase.Length-1;i++)
        {
            if (perkDatabase[i].perkName == perk.perkName)
            {
                searchPerkID = i;
                Debug.Log("ID for " + perk.perkName + " is " + searchPerkID);
                return searchPerkID;
            }
        }
        return searchPerkID;
    }
}
