using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject playerObject;
    public HealthBar playerHealth;
    public EntitySlot[] enemies;
    public static bool playerInitialized = false;
    public ItemSystem itemDatabase;
    public HealthBar[] enemyHealthBars;
    public WeaponSystem weaponDatabase;
    public EntitySystem entityDatabase;

    
    
    public Transform[] enemyBattlePositions;



    private Player player;
    public static BattleManager instance;



    void Awake()
    {
        instance = this;
        playerInitialized = false;
        player = playerObject.GetComponent<Player>();
        playerInitialized = true;
        weaponDatabase.RefreshDatabase();
        itemDatabase.RefreshDatabase();
        entityDatabase.RefreshDatabase();

        enemies = new EntitySlot[enemyBattlePositions.Length];
        enemyHealthBars = new HealthBar[enemyBattlePositions.Length];

        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = new EntitySlot();
            enemyHealthBars[i] = enemyBattlePositions[i].GetComponentInChildren<HealthBar>();
        }

    }

    void Start()
    {
        playerHealth.entity = player;
        playerHealth.ShowHealthBar();
        playerHealth.UpdateHealthBar();
    }



    void Update()
    {
        
    }

    public void ReduceEnemyHealthByOne(int healthBarNumber)
    {
        enemyHealthBars[healthBarNumber].ReduceHealth(1);
    }

    public void IncreaseEnemyHealthByOne(int healthBarNumber)
    {
        enemyHealthBars[healthBarNumber].IncreaseHealth(1);
    }

    public void TestEnemyEncounter() //debug
    {
        Debug.Log("Starting TestEnemyEncounter");
        int[] e = new int[2];
        e[0] = 2;
        e[1] = 3;
        // e[2] = 0;
        // e[3] = 0;
        SpawnEncounter(e);
    }

    public void ClearEncounter()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            RemoveEnemy(i);
        }
    }
    public void SpawnEncounter(int[] encounterList)
    {
        ClearEncounter();
        for (int i = 0; i < encounterList.Length; i++)
        {
            if (i>=enemyBattlePositions.Length)
            {
                break;
            }
            Debug.Log("attempting to make encounter enemy: " + EntitySystem.instance.GetEntity(encounterList[i]).entityName);
            CreateEntity(EntitySystem.instance.GetEntity(encounterList[i]));
        }
    }

    int CheckForEarliestEntitySlot() //returns slot number of earliest entity slot
    {
        int earliestSlot = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].taken)
            {
                Debug.Log("Earliest entity slot available is at slot " + earliestSlot);
                return earliestSlot;
            }
            earliestSlot++;
        }
        Debug.Log("No entity slots available");
        return earliestSlot;
    }

    GameObject InitializeEntity(EntityType entityType)
    {
        GameObject newEntity = new GameObject(entityType.entityName);
        SpriteRenderer sr = newEntity.AddComponent<SpriteRenderer>();
        BoxCollider2D bc2d = newEntity.AddComponent<BoxCollider2D>();
        SimpleAnimation simpleAnimation = newEntity.AddComponent<SimpleAnimation>();

        Entity e = newEntity.AddComponent<Entity>();
        e.entityType = entityType;
        e.BuildEntity();

        EnemyAI eAI = newEntity.AddComponent<EnemyAI>();
        eAI.enemy = e;
        eAI.weapon = weaponDatabase.GetWeapon("Fists");
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
                enemyHealthBars[earliestSlot].entity = newEntity.GetComponent<Entity>();
                enemyHealthBars[earliestSlot].ShowHealthBar();
                enemyHealthBars[earliestSlot].UpdateHealthBar();
                


                Debug.Log("Created entity, " + entityType.entityName + ", at slot " + earliestSlot);
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
                enemyHealthBars[slotOverride].entity = newEntity.GetComponent<Entity>();
                enemyHealthBars[slotOverride].UpdateHealthBar();
                enemyHealthBars[slotOverride].ShowHealthBar();

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
            PlayerAttackTargettingHelper.instance.HideArrow();
        }
        Destroy(enemies[slot].entityGO);
        enemies[slot].entity = null;
        enemies[slot].taken = false;
        enemyHealthBars[slot].HideHealthBar();
    }

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

    public static int selectedWeaponSlot;
    public int newSelectedWeaponSlot;

    public void SelectNewWeapon(int newWeaponInventorySlot)
    {
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            newSelectedWeaponSlot = newWeaponInventorySlot;
            Debug.Log("Hovered weapon slot: " + newSelectedWeaponSlot);
            // Debug.Log("Selected new weapon: " + WeaponInventory.instance.weapons[newSelectedWeaponSlot].weapon.itemName);
            PlayerAttackTargettingHelper.instance.ChangeCheckBool(true);
        }
    }

    public void LockInWeapon()
    {
        selectedWeaponSlot = newSelectedWeaponSlot;
        Debug.Log("Selected new weapon: " + WeaponInventory.instance.weapons[selectedWeaponSlot].weapon.itemName);
    }

    public void DEBUGCheckWinReq()
    {
        Debug.Log(checkForWinRequirements());
    }
    public bool checkForWinRequirements()
    {
        for (int i = 0; i < enemyHealthBars.Length;i++)
        {
            if (enemyHealthBars[i].entity != null)
            {
                if (enemyHealthBars[i].entity.entityType.winRequirement)
                {
                    Debug.Log("winreq enemy " + enemyHealthBars[i].entity.entityType.entityName + " found");
                    return true;
                }
            }
        }
        return false;
    }

    public void PlayerAttack(List<Entity> targets)
    {
        foreach(Entity e in targets)
        {
            for(int i = 0;i<enemyHealthBars.Length;i++)
            {
                if(enemyHealthBars[i].entity == e)
                {
                    if(WeaponInventory.instance.GetDurability(selectedWeaponSlot)>0)
                    {
                        int calcDamage = CalculateDamage(WeaponInventory.instance.weapons[selectedWeaponSlot].weapon, player, e);
                        Debug.Log("Entity, " + e.entityType.entityName + ", is being dealt " + calcDamage + " damage");
                        enemyHealthBars[i].ReduceHealth(calcDamage);
                        if (enemyHealthBars[i].GetHealth() == 0)
                        {
                            RemoveEnemy(i);
                        }
                        WeaponInventory.instance.ReduceDurability(selectedWeaponSlot,1);
                    }
                }
            }
        }
    }

    public int CalculateDamage(Weapon weapon, Entity attacker, Entity defender)
    {
        Debug.Log("Performing Damage Calculation:");
        //calc damage using attacker's atk & defender's def weapondmg*attackerpwr*(25/25+defenderdef)
        // Debug.Log(weapon.damage + " weapon damage");
        // Debug.Log(attacker.power + " attacker power");
        // Debug.Log(defender.defense + " defender defense");
        float preroundedDamage = weapon.damage * attacker.power;
        float defenseMulti = 25+defender.defense;
        // Debug.Log(defenseMulti + " defenseMulti before being divided");

        defenseMulti = 25/defenseMulti;
        // Debug.Log(preroundedDamage + " prerounded damage before defense multi");
        // Debug.Log(defenseMulti + " defenseMulti after being divided");

        preroundedDamage *= defenseMulti;
        Debug.Log(preroundedDamage + " prerounded damage");
        
        //check if special weakness, if so then do math for damage
        if (weapon.damageType == defender.entityType.weaknessTag)
        {
            Debug.Log("Weakness to " + weapon.damageType + " found!");
            preroundedDamage*=1.25f;
            Debug.Log(preroundedDamage + " prerounded damage post weakness check");
        }
        //roll for crit if can crit
        if (weapon.canCrit && attacker.canCrit)
        {
            if (RollForCrit(weapon, attacker))
            {
                Debug.Log("Landed a crit!");
                preroundedDamage*=weapon.critMult;
            }
        }
        int processedDamage = (int)preroundedDamage+1;
        //return damage
        return processedDamage;
    }


    private bool RollForCrit(Weapon weapon, Entity attacker)
    {
        float rand = Random.Range(0f, 100f);
        Debug.Log("Crit roll: " + rand);
        float critChance = attacker.luck + weapon.critChanceBoost;
        Debug.Log("Crit chance: " + critChance);
        if (critChance>=rand)
        {
            return true;
        }
        else
        return false;

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
