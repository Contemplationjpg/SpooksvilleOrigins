using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Area", menuName = "Area/Area")]
public class AreaType : ScriptableObject
{
    public EncounterType[] commonEncounters = new EncounterType[3];
    public int commonBattles = 4;
    public EncounterType[] uncommonEncounters = new EncounterType[3];
    public int uncommonBattles = 3;
    public EncounterType[] rareEncounters = new EncounterType[3];
    public int rareBattles = 2;
    public EncounterType[] legendaryEncounters = new EncounterType[3];
    public int legendaryBattles = 1;

    public Item[] areaSpecificLoot = new Item[3];


}


