using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    Player playerInfo;
    public GameObject pauseScreen;
    public static bool GamePaused = false;
    public GameObject inventoryScreen;
    public static bool InventoryOpen = false;
    public ItemSystem itemDatabase;


    public static GameManager instance;
    
    void Awake()
    {
        instance = this;
        playerInfo = player.GetComponent<Player>();
        ResumeGame();
        OpenInventory();
        CloseInventory();
        itemDatabase.RefreshDatabase();
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void OpenInventory()
    {
        inventoryScreen.SetActive(true);
        InventoryOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseInventory()
    {
        inventoryScreen.SetActive(false);
        InventoryOpen = false;
        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!InventoryOpen&&!Dialogue.dialogueRunning)
            {
                if (GamePaused)
                    ResumeGame();
                else
                    PauseGame();
            }
            
        }
        if (Input.GetButtonDown("Inventory"))
        {
            if (!GamePaused&&!Dialogue.dialogueRunning)
            {
                if (InventoryOpen)
                    CloseInventory();
                else
                    OpenInventory();
            }
        }

        

    }
    public void SaveGame(int saveNumber)
    {
        Debug.Log("Saving Game...");
        playerInfo.SavePlayer(saveNumber);
    }

    public void LoadGame(int loadNumber)
    {
        Debug.Log("Loading Game...");
        playerInfo.LoadPlayer(loadNumber);
    }

    public void ChangeScene(int SceneNumber)
    {
        SceneManager.LoadScene(sceneBuildIndex:SceneNumber, LoadSceneMode.Single);
    }

}
