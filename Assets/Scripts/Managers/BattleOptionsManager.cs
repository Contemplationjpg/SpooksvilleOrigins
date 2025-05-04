using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class BattleOptionsManager : MonoBehaviour
{
    public static BattleOptionsManager instance;
    public String GAMER;

    [SerializeField]
    public Weapon defaultWeapon, defaultWeapon2;

    public event Action OnDefaultWeaponChanged;

    public event Action OnWeaponSelectedCallback;
    public int newSelectedWeaponSlot = -1;
    public static int selectedWeaponSlot = -1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
    void UpdateDefaultWeapon()
    {
        defaultWeapon = BattleManager.instance.player.playerType.weapon;
        OnDefaultWeaponChanged.Invoke();
    }

    public void SelectNewWeapon(int newWeaponInventorySlot)
    {
        print("attempting to set newSelectedWeaponSlot to " + newWeaponInventorySlot);
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            newSelectedWeaponSlot = newWeaponInventorySlot;
            // Debug.Log("Hovered weapon slot: " + newSelectedWeaponSlot);
            // Debug.Log("Selected new weapon: " + WeaponInventory.instance.weapons[newSelectedWeaponSlot].weapon.itemName);
            PlayerAttackTargettingHelper.instance.ChangeCheckBool(true);
            OnWeaponSelectedCallback.Invoke();
        }
    }

    public void SelectDefaultWeapon()
    {
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            newSelectedWeaponSlot = -1;
            Debug.Log("Hovered weapon slot: default");
            // Debug.Log("Selected new weapon: " + WeaponInventory.instance.weapons[newSelectedWeaponSlot].weapon.itemName);
            PlayerAttackTargettingHelper.instance.ChangeCheckBool(true);
        }
    }

    public void LockInWeapon()
    {
        selectedWeaponSlot = newSelectedWeaponSlot;
        if (selectedWeaponSlot < 0)
        {
            // Debug.Log("Selected new weapon: " + defaultWeapon.itemName);
        }
        else
        Debug.Log("Selected new weapon: " + WeaponInventory.instance.weapons[selectedWeaponSlot].weapon.itemName);
    }

    public void PlayerAttack(List<Entity> targets, bool special = false)
    {
        Player player = BattleManager.instance.player;
        HealthBar playerHealth = BattleManager.instance.playerHealth;
        SugarBar playerSugar = BattleManager.instance.playerSugar;
        HealthBar[] enemyHealthBars = BattleManager.instance.enemyHealthBars;
        foreach(Entity e in targets)
        {
            for(int i = 0;i<enemyHealthBars.Length;i++)
            {
                if(enemyHealthBars[i].entity == e)
                {
                    int dealtDamage = 0;
                    if (selectedWeaponSlot<0)
                    {
                        int calcDamage = CalculateDamage(defaultWeapon, BattleManager.instance.player, e, special);
                        Debug.Log("Entity, " + e.entityType.entityName + ", is being dealt " + calcDamage + " damage");
                        dealtDamage = enemyHealthBars[i].ReduceHealth(calcDamage);
                    }
                    else
                    {
                        if(WeaponInventory.instance.GetDurability(selectedWeaponSlot)>0)
                        {
                            int calcDamage = CalculateDamage(WeaponInventory.instance.weapons[selectedWeaponSlot].weapon, player, e, special);
                            Debug.Log("Entity, " + e.entityType.entityName + ", is being dealt " + calcDamage + " damage");
                            dealtDamage = BattleManager.instance.enemyHealthBars[i].ReduceHealth(calcDamage);
                            WeaponInventory.instance.ReduceDurability(selectedWeaponSlot, WeaponInventory.instance.weapons[selectedWeaponSlot].weapon.attackDurabilityCost);
                        }
                    }
                    BattleManager.instance.SpawnEffectText(dealtDamage.ToString(),i,"white");

                    if (player.vamp)
                    {
                        playerHealth.IncreaseHealth((int)Math.Round(dealtDamage*player.vampAmount));
                    }
                    if (player.sugarSteal)
                    {
                        playerSugar.IncreaseSugar((int)Math.Round(dealtDamage*player.sugarStealAmount));
                    }
                    
                    

                    if (enemyHealthBars[i].GetHealth() == 0)
                    {
                        Debug.Log("Enemy killed at slot " + i);
                        BattleManager.instance.InvokeEnemyKilled();
                        BattleManager.instance.RemoveEnemy(i);
                    }
                            
                    
                }
            }
        }
    }

    public int CalculateDamage(Weapon weapon, Entity attacker, Entity defender, bool special = false)
    {
        // Debug.Log("Performing Damage Calculation:");
        //calc damage using attacker's atk & defender's def weapondmg*attackerpwr*(25/25+defenderdef)
        // Debug.Log(weapon.damage + " weapon damage");
        // Debug.Log(attacker.power + " attacker power");
        // Debug.Log(defender.defense + " defender defense");
        float preroundedDamage;
        float attackerPowerAfterBuffs = (attacker.power + attacker.powerFlatMod)*attacker.powerMultMod;
        float defenderDamageAfterBuffs = (defender.defense + defender.defenseFlatMod)*defender.defenseMultMod;
        if (!special)
        preroundedDamage = weapon.damage * attackerPowerAfterBuffs;
        else
        preroundedDamage = weapon.specialDamage * attackerPowerAfterBuffs;
        float defenseMulti = 25+defenderDamageAfterBuffs;
        // Debug.Log(defenseMulti + " defenseMulti before being divided");

        defenseMulti = 25/defenseMulti;
        // Debug.Log(preroundedDamage + " prerounded damage before defense multi");
        // Debug.Log(defenseMulti + " defenseMulti after being divided");

        preroundedDamage *= defenseMulti;
        // Debug.Log(preroundedDamage + " prerounded damage");
        
        //check if special weakness, if so then do math for damage
        if (weapon.damageType == defender.entityType.weaknessTag && weapon.damageType != "")
        {
            // Debug.Log("Weakness to " + weapon.damageType + " found!");
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
        if (processedDamage <= 0)
        {
            processedDamage = 1;
        }
        return processedDamage;
    }


    private bool RollForCrit(Weapon weapon, Entity attacker)
    {
        float rand = UnityEngine.Random.Range(0f, 100f);
        // Debug.Log("Crit roll: " + rand);
        float critChance = attacker.luck + weapon.critChanceBoost;
        // Debug.Log("Crit chance: " + critChance);
        if (critChance>=rand)
        {
            return true;
        }
        else
        return false;

    }

    public void EatWeaponForTurn()
    {
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            LockInWeapon();
            TurnManager.instance.choice = TurnManager.Choice.Eat;
            TurnManager.instance.ChoiceChosen = true;
        }
        
    }

    public bool EatWeapon(int eatWeaponSlot = -1)
    {
        if (eatWeaponSlot<0)
        {
            eatWeaponSlot = selectedWeaponSlot;
        }
        if (WeaponInventory.instance.weapons[eatWeaponSlot].durability>0)
        {
            WeaponInventory.instance.ReduceDurability(eatWeaponSlot, 1);
            BattleManager.instance.playerSugar.IncreaseSugar(WeaponInventory.instance.weapons[eatWeaponSlot].weapon.sugarYield);
            return true;
        }
        else
        {
            return false;
        }
        
        
    }

    public void PassTurn()
    {
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            TurnManager.instance.choice = TurnManager.Choice.PassTurn;
            TurnManager.instance.ChoiceChosen = true;
        }
    }

    public Weapon GetDefaultWeapon()
    {
        if (defaultWeapon == null) {
            print("TRYING TO GET DEFAULT WEAPON!!!");
            defaultWeapon = BattleManager.instance.player.playerType.weapon;
            print("DEFAULT WEAPON: " + defaultWeapon.name);

        }
        return defaultWeapon;
    }
}
