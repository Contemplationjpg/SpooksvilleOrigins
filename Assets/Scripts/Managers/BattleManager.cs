using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject playerObject;
    public HealthBar playerHealth;
    public SugarBar playerSugar;
    public GameObject effectTextPrefab;
    public EntitySlot[] enemies;
    public bool playerInitialized = false;
    public ItemSystem itemDatabase;
    public HealthBar[] enemyHealthBars;
    public WeaponSystem weaponDatabase;
    public EntitySystem entityDatabase;
    public EncounterSystem encounterDatabase;
    public PerkSystem perkDatabase;


    public EncounterType currentEncounter;
    public int encounterNumber = 0;
    public EncounterType[] encounterList = new EncounterType[1];

    
    
    public Transform[] enemyBattlePositions;

    public event Action OnEnemyKilled;



    public Player player;
    public static BattleManager instance;




    void Awake()
    {
        instance = this;

        //initialize player so that other methods don't try to refer to player it is build
        playerInitialized = false;
        player = playerObject.GetComponent<Player>();
        player.BuildEntity();
        player.BuildPlayerEntity();
        playerInitialized = true;

        //refresh all databases
        weaponDatabase.RefreshDatabase();
        itemDatabase.RefreshDatabase();
        entityDatabase.RefreshDatabase();
        encounterDatabase.RefreshDatabase();
        perkDatabase.RefreshDatabase();

        //creates array of EntitySlots, each holds reference to if spot taken, entity, and entity GameObject
        enemies = new EntitySlot[enemyBattlePositions.Length];
        //creates array of healthbars
        enemyHealthBars = new HealthBar[enemyBattlePositions.Length];

        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new EntitySlot(); //create new entityslots
            enemyHealthBars[i] = enemyBattlePositions[i].GetComponentInChildren<HealthBar>(); //grab reference to healthbar from enemy GameObjects
        }
    }

    void Start()
    {
        InitializePlayerHealthBar();
        InitializePlayerSugarBar();
        SpawnEncounter();
        // SetEncounterLoot();
        StartCoroutine(LateStartForAddingWeapon());//debug
        SeedManager.instance.GenerateRandomSeed();
    }


    IEnumerator LateStartForAddingWeapon() //debug
    { 
        yield return new WaitForSeconds(1f);
        WeaponInventory.instance.AddTestWeapon();
        TurnManager.instance.StartBattle();
    }


    void Update()
    {
        
    }

    void InitializePlayerHealthBar()
    {
        playerHealth.ChangeEntity(player);
        playerHealth.ShowHealthBar();
        playerHealth.UpdateHealthBar();
    }

    void InitializePlayerSugarBar()
    {
        playerSugar.entity = player;
        playerSugar.ShowSugarBar();
        playerSugar.UpdateSugarBar();
    }

    public void ReduceEnemyHealthByOne(int healthBarNumber)
    {
        enemyHealthBars[healthBarNumber].ReduceHealth(1);
    }

    public void IncreaseEnemyHealthByOne(int healthBarNumber)
    {
        enemyHealthBars[healthBarNumber].IncreaseHealth(1);
    }

    // public void TestEnemyEncounter()
    // {
    //     Debug.Log("Starting TestEnemyEncounter");
    //     int[] e = new int[2];
    //     e[0] = 1;
    //     e[1] = 1;
    //     // e[2] = 0;
    //     // e[3] = 0;
    //     SpawnEncounter(e);
    // }

    public bool SpawnEncounter()
    {
        if (encounterNumber > encounterList.Length-1)
        {
            return false;
        }

        ClearEnemies();
        currentEncounter = encounterList[encounterNumber];

        for (int i = 0; i < enemies.Length; i++)
        {
            RemoveEnemy(i);
        }
        for (int i = 0; i < currentEncounter.enemies.Length; i++)
        {
            if (i>=enemyBattlePositions.Length)
            {
                break;
            }
            // Debug.Log("attempting to make encounter enemy: " + currentEncounter.enemies[i].entityName);
            CreateEntity(currentEncounter.enemies[i]);
        }
        for (int i = 0; i < currentEncounter.killReqs.Length; i++) 
        {
            enemies[currentEncounter.killReqs[i]].entity.requiredKill = true;
        }
        return true;
    }

    public void SetEncounterLoot()
    {
        LootManager.instance.SetAllLoot(currentEncounter);
    }

    public void increaseEncounterNumber(int num = 1) 
    {
        encounterNumber += num;
    }

    public int CheckForEarliestEntitySlot() //returns slot number of earliest entity slot
    {
        int earliestSlot = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].taken)
            {
                // Debug.Log("Earliest entity slot available is at slot " + earliestSlot);
                return earliestSlot;
            }
            earliestSlot++;
        }
        Debug.Log("No entity slots available");
        return earliestSlot;
    }

    public bool CheckForKillRequirement() //returns slot number of earliest entity slot
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].taken)
            {
                if (enemies[i].entity.requiredKill)
                {
                    // Debug.Log("Kill requirement exists at slot " + i);
                    return true;
                }
            }
        }
        Debug.Log("No kill requirement entity exists");
        return false;
    }

    public void InvokeEnemyKilled()
    {
        OnEnemyKilled.Invoke();
    }

    GameObject InitializeEntity(EntityType entityType)
    {
        GameObject newEntity = new GameObject(entityType.entityName);
        SpriteRenderer sr = newEntity.AddComponent<SpriteRenderer>();
        sr.flipX = true; //flips x (intended for enemies)
        BoxCollider2D bc2d = newEntity.AddComponent<BoxCollider2D>();
        SimpleAnimation simpleAnimation = newEntity.AddComponent<SimpleAnimation>(); //replace to change animation

        Entity e = newEntity.AddComponent<Entity>();
        e.entityType = entityType;
        e.BuildEntity();

        EnemyAI eAI = newEntity.AddComponent<EnemyAI>();
        eAI.enemy = e;
        eAI.weapon = e.entityType.weapon; //weaponDatabase.GetWeapon("Fists");
        eAI.playerHealthBar = playerHealth;
        eAI.player = playerObject.GetComponent<Player>();
        eAI.SetUp();

        e.ai = eAI;


        bc2d.isTrigger=true;
        bc2d.size = sr.size;

        return newEntity;
    }


    public bool CreateEntity(EntityType entityType, int slotOverride = 0)
    {
        if (slotOverride < 0)
            slotOverride = 0;

        
        if (slotOverride==0)
        {
            int earliestSlot = CheckForEarliestEntitySlot();
            if (earliestSlot<=enemies.Length-1)
            {
                // GameObject newEntity = new GameObject(entityType.entityName);
                // SpriteRenderer sr = newEntity.AddComponent<SpriteRenderer>();
                // BoxCollider2D bc2d = newEntity.AddComponent<BoxCollider2D>();
                // Entity e = newEntity.AddComponent<Entity>();
                // e.entityType = entityType;

                // e.BuildEntity();

                // bc2d.isTrigger=true;
                // bc2d.size = sr.size;

                GameObject newEntity = InitializeEntity(entityType);

                newEntity.transform.position = enemyBattlePositions[earliestSlot].transform.position;
                enemies[earliestSlot].entity = newEntity.GetComponent<Entity>();
                enemies[earliestSlot].entityGO = newEntity;
                enemies[earliestSlot].taken = true;
                enemyHealthBars[earliestSlot].ChangeEntity(newEntity.GetComponent<Entity>());
                enemyHealthBars[earliestSlot].ShowHealthBar();
                enemyHealthBars[earliestSlot].UpdateHealthBar();
                enemyHealthBars[earliestSlot].SetWeapon();
                


                // Debug.Log("Created entity, " + entityType.entityName + ", at slot " + earliestSlot);
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough room to create entity: " + entityType.entityName);
                return false;
            }

        }
        else //if there is a slot override
        {
            if (slotOverride>0)
            slotOverride--;
            if (!enemies[slotOverride].taken)
            {

            GameObject newEntity = InitializeEntity(entityType);

                newEntity.transform.position = enemyBattlePositions[slotOverride].transform.position;
                enemies[slotOverride].entity = newEntity.GetComponent<Entity>();
                enemies[slotOverride].entityGO = newEntity;
                enemies[slotOverride].taken = true;
                enemyHealthBars[slotOverride].ChangeEntity(newEntity.GetComponent<Entity>());
                enemyHealthBars[slotOverride].UpdateHealthBar();
                enemyHealthBars[slotOverride].ShowHealthBar();
                enemyHealthBars[slotOverride].SetWeapon();

                Debug.Log("Created entity, " + entityType.entityName + ", at slot " + slotOverride);
                return true;
            }
            else
            {
                Debug.LogWarning("No space to create entity, " + entityType.entityName + ", at slot " + slotOverride);
                return false;
            }
        }
    }

    public void ForceCreateEntity(EntityType entityType)
    {
        CreateEntity(entityType);
    }

    public void RemoveEnemy(int slot)
    {
        if(PlayerAttackTargettingHelper.instance.GetTarget()==enemies[slot].entityGO)
        {
            PlayerAttackTargettingHelper.instance.UpdateVisibility();
        }
        Destroy(enemies[slot].entityGO);
        enemies[slot].entity = null;
        enemies[slot].taken = false;
        enemyHealthBars[slot].HideHealthBar();
    }

    public void ClearEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            RemoveEnemy(i);
        }
    }

    // public void KillEnemy(int slot)
    // {

    //     RemoveEnemy(slot);
    // }

    public int FindEnemyInSlot(Entity entCheck)
    {
        int enemySlot = enemyBattlePositions.Length+1;

        for (int i = 0; i<enemyHealthBars.Length;i++)
        {
            if (enemyHealthBars[i].entity != null)
            {
                if (entCheck == enemyHealthBars[i].entity)
                {
                    return i;
                }
            }
        }

        return enemySlot;
    }

    

    
    public void SpawnEffectText(String text,int slot = -1, String color = "white")
    {
        GameObject effText = Instantiate(effectTextPrefab);
        effText.GetComponent<EffectText>().SetString(text);
        // UnityEngine.Vector3 locScale = new UnityEngine.Vector3(0.6f, 0.6f, 0.6f);
        // effText.GetComponent<Transform>().localScale = locScale;
        if (color.Equals("white"))
        {
            effText.GetComponent<TMP_Text>().color = Color.white;
        }
        else if (color.Equals("red"))
        {
            effText.GetComponent<TMP_Text>().color = Color.red;
        }
        else if (color.Equals("blue"))
        {   
            effText.GetComponent<TMP_Text>().color = Color.blue;
        }
        else if (color.Equals("green"))
        {   
            effText.GetComponent<TMP_Text>().color = Color.green;
        }
        else if (color.Equals("grey"))
        {   
            effText.GetComponent<TMP_Text>().color = Color.grey;
        }

        var randX = UnityEngine.Random.Range(-0.1f,0.1f);
        var randY = UnityEngine.Random.Range(-0.1f,0.1f);

        if (slot<0)
        {
            UnityEngine.Vector2 effTextPos = new UnityEngine.Vector2(playerObject.transform.position.x+randX,playerObject.transform.position.y+0.2f+randY);
            effText.GetComponent<Transform>().position = effTextPos;
        }
        else
        {
            UnityEngine.Vector2 effTextPos = new UnityEngine.Vector2(enemyBattlePositions[slot].transform.position.x+randX,enemyBattlePositions[slot].transform.position.y+0.3f+randY);
            effText.GetComponent<Transform>().position = effTextPos;
        }
    }






    public void SaveGame(int saveNumber)
    {
        Debug.Log("Saving Game...");
        player.SavePlayer(saveNumber);
    }

    public void LoadGame(int loadNumber)
    {
        Debug.Log("Loading Game...");
        player.LoadPlayer(loadNumber);
    }


}

public class EntitySlot
{
    public bool taken = false;
    public Entity entity = null;
    public GameObject entityGO = null;

}
