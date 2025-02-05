using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TurnManager : MonoBehaviour
{
    public State state;
    public Choice choice;
    public bool ChoiceChosen = false;
    
    public TMP_Text actionsDisplay;
    public TMP_Text turnDisplay;
    public Player player;
    public Item selectedItem;


    public enum State { None, Busy, WaitingForPlayerInput, WaitingForEnemyInput, Ending, Win }

    public enum Choice { Attack, SpecialAttack, Eat, UseItem, Nothing, PassTurn }

    public static TurnManager instance;
    private bool checkingForEnemyAction = false;
    public bool doingEnemyAction = false;
    public bool playerDefending = false;
    public float playerDefendingMod = 1f;
    public event Action PlayerActionable;
    public event Action OnUpdateHealthStats;
    public event Action PlayerNonActionable;
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        ChangeState(State.None);
    }

    public void StartBattle()
    {
        TooltipManager.instance.EntityStatTip(player);
        ChangeState(State.WaitingForPlayerInput);
        PlayerActionable.Invoke();
    }

    public void UpdateTurnDisplay()
    {
        turnDisplay.text = "Game State: " + state; 
        actionsDisplay.text = "Actions Remaining: " + player.currentActionCount;
    }
    public void ChangeState(State changeTo)
    {
        state = changeTo;
        UpdateTurnDisplay();
    }

    public void ChangePlayerActionable(bool toggle)
    {
        if (toggle)
        PlayerActionable.Invoke();
        else
        PlayerNonActionable.Invoke();
    }

    public void ActivatePlayerDefending(float defMod)
    {
        Debug.Log("activating player defense");
        playerDefendingMod = defMod;
        for (int i = 0; i < player.currentActionCount; i++)
        {
            playerDefendingMod*=defMod;
        }
        BattleManager.instance.SpawnEffectText("Blocking x"+player.currentActionCount+"!",-1,"grey");
        playerDefending = true;
    }

    private void Update()
    {
        if(state!=State.None)
        {
            if (state == State.WaitingForPlayerInput)
            {
                if (player.currentActionCount<=0)
                {   
                    ChangeState(State.WaitingForEnemyInput);
                }
                else if (ChoiceChosen)
                {
                    ChoiceChosen = false;
                    ChangeState(State.Busy);
                    switch ((int)choice)
                    {
                    case 0: //Attack
                    player.GetComponentInParent<SimpleAnimation>().DoLittleHop();
                    BattleManager.instance.PlayerAttack(PlayerAttackTargettingHelper.instance.targets);
                    PlayerAttackTargettingHelper.instance.targets.Clear(); 
                    
                    player.ChangeActionCount(-1);
                    UpdateTurnDisplay();
                    // StartCoroutine(FinishTurn(player, State.WaitingForEnemyInput));
                    StartCoroutine(ContinuePlayerTurn());
                    
                    return;

                    case 1: //SpecialAttack

                    player.GetComponentInParent<SimpleAnimation>().DoBigHop();
                    BattleManager.instance.PlayerAttack(PlayerAttackTargettingHelper.instance.targets, true);
                    PlayerAttackTargettingHelper.instance.targets.Clear(); 
                    
                    BattleManager.instance.playerSugar.ReduceSugar(WeaponInventory.instance.weapons[BattleManager.instance.newSelectedWeaponSlot].weapon.specialSugarCost);
                    player.ChangeActionCount(-1);
                    UpdateTurnDisplay();
                    // StartCoroutine(FinishTurn(player, State.WaitingForEnemyInput));
                    StartCoroutine(ContinuePlayerTurn());
                    return;

                    case 2: //Eat
                    if (BattleManager.instance.EatWeapon())
                    {
                        player.ChangeActionCount(-1);
                        UpdateTurnDisplay();
                    }
                    StartCoroutine(ContinuePlayerTurn());
                    
                    return;

                    case 3: //UseItem
                    if (selectedItem.AttemptItemUse())
                    {
                        player.ChangeActionCount(-1);
                        UpdateTurnDisplay();
                    }
                    StartCoroutine(ContinuePlayerTurn());

                    return;

                    case 4: //Nothing (AKA the choice wasn't valid)

                    return;

                    case 5: //pass turn
                    PassTurn();

                    return;

                    default: //even more nothing????
                    return;

                    }
                    
                }
                
            }
            else if (state == State.WaitingForEnemyInput) //enemy turn
            {
                ChangeState(State.Busy);
                
                StartCoroutine(DoEnemyTurns());



            }
            else if (state == State.Ending)
            {
                if(BattleManager.instance.CheckForKillRequirement()) 
                {
                        PlayerActionable.Invoke();
                        ChangeState(State.WaitingForPlayerInput);
                }
                else
                {
                    ChangeState(State.None); //this should be when loot table should come up
                    PlayerNonActionable.Invoke();
                    LootManager.instance.StartLooting();
                }
            }
        }
    }
IEnumerator FinishTurn(Entity entity, State nextState)
{
    SimpleAnimation animation = entity.GetComponentInParent<SimpleAnimation>();
    yield return new WaitUntil(() => !animation.doingAnimation);
    
    ChangeState(nextState);

    yield break;
}

IEnumerator ContinuePlayerTurn()
{
    SimpleAnimation animation = player.GetComponentInParent<SimpleAnimation>();
    yield return new WaitUntil(() => !animation.doingAnimation);
    OnUpdateHealthStats.Invoke();
    if(BattleManager.instance.CheckForKillRequirement()) 
    {
        if (player.currentActionCount>0)
        {
            PlayerActionable.Invoke();
            ChangeState(State.WaitingForPlayerInput);
        }
        else
        {
            PlayerNonActionable.Invoke();
            ChangeState(State.WaitingForEnemyInput);
        }
    }
    else
    {
        PlayerNonActionable.Invoke();
        ChangeState(State.Ending);
    }
    yield break;
}

public void PassTurn()
{
    if (player.passBlock)
    {
        ActivatePlayerDefending(player.passBlockReduction);
    }
    ChangeState(State.WaitingForEnemyInput);
}

IEnumerator DoEnemyTurns()
{
    checkingForEnemyAction = true;
    for (int i = 0; i < BattleManager.instance.enemyHealthBars.Length; i++)
    {
        yield return new WaitUntil(() => checkingForEnemyAction);
        if (BattleManager.instance.enemyHealthBars[i].entity !=null)
        {
            checkingForEnemyAction = false;
            Entity currentEnemy = BattleManager.instance.enemyHealthBars[i].entity;
            // BattleManager.instance.enemyHealthBars[i].entity.ai.EnemyDecision();
            // SimpleAnimation.instance.DoLittleHop(BattleManager.instance.enemyHealthBars[i].entity.ai.GetComponentInParent<Transform>());
            SimpleAnimation anima = currentEnemy.GetComponentInParent<SimpleAnimation>();
            // anima.DoLittleHop();
            currentEnemy.ai.EnemyDecision();
            OnUpdateHealthStats.Invoke();
            yield return new WaitUntil(() => !anima.doingAnimation);
            
            Debug.Log("Enemy " + i + " turn over.");
            checkingForEnemyAction = true;
        }
        else
        {
            checkingForEnemyAction = true;
        }
    }
    checkingForEnemyAction = false;
    player.ResetActionCount();
    playerDefending = false;
    ChangeState(State.Ending);

}



}