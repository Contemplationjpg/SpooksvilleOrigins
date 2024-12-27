using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Entity/Encounter")]
public class EncounterType : ScriptableObject
{
     public string encounterName = "default name";
    public EntityType[] enemies;
    public int[] killReqs = new int[1];
    public Item[] loot = new Item[3]; 
    public int[] lootCounts = new int[3];
}
