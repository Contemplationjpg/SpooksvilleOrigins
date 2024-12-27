using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "New EncounterDatabase", menuName = "Encounter/EncounterDatabase")]
public class EncounterSystem : ScriptableObject
{
    public EncounterType[] encounterDatabase;
    public static EncounterSystem instance;

    public void RefreshDatabase()
    {
        instance = this;
    }

    public EncounterType GetEncounter(int encounterID)
    {   
        EncounterType getEncounter = CreateInstance<EncounterType>();

        if (encounterID < encounterDatabase.Length)
        {
            getEncounter = encounterDatabase[encounterID];
            Debug.Log("encounter at ID " + encounterID + " is " + getEncounter.encounterName);
        }
        
        return getEncounter;
    }

    public EncounterType GetEncounter(string encounterName)
    {
        EncounterType getEncounter = CreateInstance<EncounterType>();
        
        for (int i = 0; i < encounterDatabase.Length;i++)
        {
            if (encounterName == encounterDatabase[i].encounterName)
            {
                getEncounter = encounterDatabase[i];
                return getEncounter;
            }
        }

        return getEncounter;
    }

    public int GetEncounterID(EncounterType encounter)
    {
        int searchEncounterID = 0;

        for(int i = 0; i < encounterDatabase.Length-1;i++)
        {
            if (encounterDatabase[i].encounterName == encounter.encounterName)
            {
                searchEncounterID = i;
                Debug.Log("ID for " + encounter.encounterName + " is " + searchEncounterID);
                return searchEncounterID;
            }
        }
        return searchEncounterID;
    }
}
