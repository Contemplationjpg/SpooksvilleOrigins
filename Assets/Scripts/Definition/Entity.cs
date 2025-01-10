using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour
{
    public EntityType entityType = null;
    public string entityName = "default name";
    public bool isEnemy = true;
    public EnemyAI ai;
    public bool requiredKill = false;


    public int maxHealth;
    public int currentHealth;
    public bool canOverheal;
    public bool hasOverhealLimit;
    public int overhealLimit = 0;


    //power, def, type, luck, attack delay (turns between attacks)

    public int defense = 10;
    public int defenseFlatMod = 0;
    public float defenseMultMod = 0;
    public int power = 10;
    public int powerFlatMod = 0;
    public float powerMultMod = 0;
    public float luck = 1; //percent chance of critting
    public bool canCrit = true;
    public int attackDelay = 1;

    public void BuildEntity()
    {
        if(entityType!=null)
        {
            entityName = entityType.entityName;
            GetComponent<SpriteRenderer>().sprite = entityType.sprite;
            isEnemy = entityType.isEnemy;

            //stats
            maxHealth = entityType.maxHealth;
            currentHealth = entityType.currentHealth;
            canOverheal = entityType.canOverheal;
            hasOverhealLimit = entityType.hasOverhealLimit;
            overhealLimit = entityType.overhealLimit;
            defense = entityType.defense;
            power = entityType.power;
            luck = entityType.luck;
            canCrit = entityType.canCrit;
            attackDelay = entityType.attackDelay;
            ResetAllBuffs();
        }
    }

    public void ResetPowerBuffs()
    {
        powerFlatMod = 0;
        powerMultMod = 1;
    }
    public void ResetDefenseBuffs()
    {
        defenseFlatMod = 0;
        defenseMultMod = 1;
    }
    public void ResetAllBuffs()
    {
        ResetPowerBuffs();
        ResetDefenseBuffs();
    }

    void OnMouseEnter()
    {
        if (PlayerAttackTargettingHelper.checkingForMouse&&isEnemy)
        {
            // Debug.Log("Mouse Hovered: " + entityName);
            PlayerAttackTargettingHelper.instance.UpdateArrowPosition(gameObject);
        }
        // Debug.Log("mouse entered entity");
        // if (!TooltipManager.instance.expanded)
        // {
        //     TooltipManager.instance.EntityStatTip(this);
        // }
        
    }

    // void OnMouseExit()
    // {
    //     // Debug.LogWarning("mouse exited entity");
    //     // TooltipManager.instance.HideEntityTip();
    // }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TooltipManager.instance.EntityStatTip(this);
        }
    }

    void OnMouseDown()
    {
        if (PlayerAttackTargettingHelper.checkingForMouse&&TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput&&isEnemy)  //&&BattleManager.instance.newSelectedWeaponSlot<=WeaponInventory.instance.weapons.Length)
        {
            if (BattleManager.instance.newSelectedWeaponSlot < 0)
            {
                PlayerAttackTargettingHelper.instance.AttemptToAddTargetToList(this);
                BattleManager.instance.LockInWeapon();
                TurnManager.instance.choice = TurnManager.Choice.Attack;
                TurnManager.instance.ChoiceChosen = true;
            }
            else
            if (BattleManager.instance.newSelectedWeaponSlot<=WeaponInventory.instance.weapons.Length)
            {
                if (WeaponInventory.instance.weapons[BattleManager.instance.newSelectedWeaponSlot].weapon.attackDurabilityCost <= WeaponInventory.instance.weapons[BattleManager.instance.newSelectedWeaponSlot].durability)
                {
                    PlayerAttackTargettingHelper.instance.AttemptToAddTargetToList(this);
                    if (PlayerAttackTargettingHelper.instance.doingSpecial)
                    {
                        if (WeaponInventory.instance.weapons[BattleManager.instance.newSelectedWeaponSlot].weapon.specialSugarCost <= TurnManager.instance.player.currentSugar)
                        {
                            BattleManager.instance.LockInWeapon();
                            TurnManager.instance.choice = TurnManager.Choice.SpecialAttack;
                            TurnManager.instance.ChoiceChosen = true;
                        }
                        else
                        {
                            PlayerAttackTargettingHelper.instance.ClearTargetList();
                            TurnManager.instance.choice = TurnManager.Choice.Nothing;
                        }
                    }
                    else
                    {
                        BattleManager.instance.LockInWeapon();
                        TurnManager.instance.choice = TurnManager.Choice.Attack;
                        TurnManager.instance.ChoiceChosen = true;
                    }
                }
            }
        }
        // else
        // {
        //     if (TooltipManager.instance.expanded)
        //     {
        //         TooltipManager.instance.EntityStatTip(this);
        //     }
        // }
    }


}
