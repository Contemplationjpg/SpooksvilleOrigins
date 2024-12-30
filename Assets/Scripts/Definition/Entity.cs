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


    //power, def, type, luck, attack delay (turns between attacks)

    public int defense = 10;
    public int power = 10;
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
            defense = entityType.defense;
            power = entityType.power;
            luck = entityType.luck;
            canCrit = entityType.canCrit;
            attackDelay = entityType.attackDelay;
        }
    }

    void OnMouseEnter()
    {
            if (PlayerAttackTargettingHelper.checkingForMouse&&isEnemy)
            {
                Debug.Log("Mouse Hovered: " + entityName);
                PlayerAttackTargettingHelper.instance.UpdateArrowPosition(gameObject);
            }
    }

    void OnMouseDown()
    {
        if (PlayerAttackTargettingHelper.checkingForMouse&&TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)  //&&BattleManager.instance.newSelectedWeaponSlot<=WeaponInventory.instance.weapons.Length)
        {
            if (BattleManager.instance.newSelectedWeaponSlot<=WeaponInventory.instance.weapons.Length)
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


}
