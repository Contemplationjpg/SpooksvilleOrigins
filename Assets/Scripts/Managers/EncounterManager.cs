using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    LootManager lootManager;
    GameManager gameManager;


    public void Start()
    {
        lootManager = LootManager.instance;
        gameManager = GameManager.instance;
    }

    
}
