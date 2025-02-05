using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public class Player : Entity
{
    public bool loadSaveOnAwake = false;
    public int loadNumber = 1;

    public PlayerType playerType = null;

    public int maxSugar = 100;
    public int currentSugar = 0;
    public int maxActionCount = 3;
    public int currentActionCount = 3;

    [Header("Character Passives")]
    public bool ghost = false;
    public float ghostChance = 0.75f;
    public bool vamp = false;
    public float vampAmount = 0.25f;
    public bool wolf = false;
    [Header("Perks")]
    public bool sugarSteal = false;
    public float sugarStealAmount = 0.25f;
    public bool passBlock = false;
    public float passBlockReduction = 0.75f;



    
    public void Awake()
    {
        entityName = "Player";
        entityType = playerType;

        if (loadSaveOnAwake)
        LoadPlayer(loadNumber);

        maxSugar = playerType.maxSugar;
        // Debug.Log("player max sugar set to " + maxSugar);
        currentSugar = playerType.currentSugar;
        // Debug.Log("player current sugar set to " + currentSugar);
        maxActionCount = playerType.baseActionCount;
        currentActionCount = maxActionCount;
        // Debug.Log("player action count set to " + actionCount);
        BuildEntity();
    }

    public void BuildPlayerEntity()
    {
        ghost = playerType.ghost;
        ghostChance = playerType.ghostChance;
        vamp = playerType.vamp;
        vampAmount = playerType.vampAmount;
        wolf = playerType.wolf;
    }

    public bool ChangeActionCount(int num)
    {
        if (currentActionCount+num < 0)
        {
            return false;
        }
        currentActionCount+=num;
        return true;
    }

    public bool ChangeSugarAmount(int num)
    {
        if (currentSugar+num < 0)
        {
            return false;
        }
        currentSugar+=num;
        return true;
    }

    public void ResetActionCount()
    {
        currentActionCount = maxActionCount;
    }


    public void SavePlayer(int saveNumber)
    {
        PlayerData currentData = new PlayerData(this);

        bool saved = SerializationManager.Save(saveNumber, currentData);
        if (saved)
        Debug.Log("Game Saved.");
        else
        Debug.LogWarning("Error Saving Game");
    }

    public void LoadPlayer(int saveNumber)
    {
        string path = Application.persistentDataPath + "/saves/" + "save" + saveNumber.ToString() + ".save";
        PlayerData data = SerializationManager.Load(path);
        if (data==null)
        {
            _ = new PlayerData(this);
            Debug.Log("did not load new data");
            return;
        }

        Debug.Log("setting maxHealth to " + data.maxHealth);
        maxHealth = data.maxHealth;
        
        Debug.Log("setting currentHealth to " + data.currentHealth);
        currentHealth = data.currentHealth;
        
        if (SceneManager.GetActiveScene().name == "Overworld" || SceneManager.GetActiveScene().name == "RestArea")
        {
            Vector2 position = new Vector2(data.position[0], data.position[1]);
            Debug.Log("setting position to " + data.position[0] + ", " + data.position[1]);
            transform.position = position;
        }

        Debug.Log("rebuilding inventory...");
        Inventory.instance.LoadInventory(data.ItemNames, data.ItemCounts);
        

        Debug.Log("Loaded Save " + saveNumber);
    }


}
