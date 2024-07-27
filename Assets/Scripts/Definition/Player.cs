using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public class Player : Entity
{
    public bool loadSaveOnAwake = false;
    public int loadNumber = 1;

    
    public void Awake()
    {
        entityName = "Player";
        if (loadSaveOnAwake)
        LoadPlayer(loadNumber);
        BuildEntity();
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
