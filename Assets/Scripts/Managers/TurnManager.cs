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
    
    public TMP_Text turnDisplay;
    public Player player;


    public enum State { None, Busy, WaitingForPlayerInput, WaitingForEnemyInput, Ending, Win }

    public enum Choice { Attack, Item }

    public static TurnManager instance;
    private bool checkingForEnemyAction = false;
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        ChangeState(State.None);
        StartBattle();
    }

    public void StartBattle()
    {
        // ChangeState(State.WaitingForPlayerInput);
        ChangeState(State.NoEncounter);
    }

    public void UpdateTurnDisplay()
    {
        turnDisplay.text = "Game State: " + state; 
    }
    public void ChangeState(State changeTo)
    {
        Debug.Log("Changing State to " + changeTo);
        state = changeTo;
        UpdateTurnDisplay();
    }

    private void Update()
    {
        if(state!=State.None)
        {
            if (state == State.NoEncounter)
            {
                ChangeState(State.Busy);
                BattleManager.instance.TestEnemyEncounter(); //debug
                ChangeState(State.WaitingForPlayerInput);
            }
            if (state == State.EncounterWin)
            {
                ChangeState(State.Busy);
                LootManager.instance.EncounterLoot();
            }
            if (state == State.WaitingForPlayerInput)
            {
                if (ChoiceChosen)
                {
                    ChoiceChosen = false;
                    ChangeState(State.Busy);
                    switch ((int)choice)
                    {
                    case 0: //Attack
                    player.GetComponentInParent<SimpleAnimation>().DoLittleHop();
                    BattleManager.instance.PlayerAttack(PlayerAttackTargettingHelper.instance.targets);
                    PlayerAttackTargettingHelper.instance.targets.Clear(); 
                    
                    StartCoroutine(FinishTurn(player, State.WaitingForEnemyInput));


                    return;

                    case 1: //Item

                    return;

                    default: //nothing????
                    return;
                    }
                }
            }
            else if (state == State.WaitingForEnemyInput)
            {
                ChangeState(State.Busy);
                if (!BattleManager.instance.checkForWinRequirements())
                {
                    ChangeState(State.EncounterWin);
                    BattleManager.instance.ClearEncounter();
                }
                else
                {
                    StartCoroutine(DoEnemyTurns());

                }
                
                // if (ChoiceChosen)
                // {
                //     switch ((int)choice)
                //     {
                //     case 0: //Attack

                //     return;

                //     case 1: //Item

                //     return;

                //     default: //nothing????
                //     return;
                //     }
                // }
            }
            else if (state == State.Ending)
            {
                if(BattleManager.instance.CheckForKillRequirement()) 
                {
                    ChangeState(State.WaitingForPlayerInput);
                }
                else
                {
                    ChangeState(State.None); //this should be when loot table should come up
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
            anima.DoLittleHop();
            currentEnemy.ai.EnemyAttack();
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
    ChangeState(State.Ending);
}



}





// public enum TurnHolder {PreTurn, Player, Enemy, EndTurn, PostTurn}
// public enum TurnAction {None, Attack, Special, Consume, Item, Unique}
// public class TurnManager : MonoBehaviour
// {
//     [SerializeField]
//     TMP_Text turnTrackerText;
//     public static TurnHolder turnHolder;
//     public static TurnAction playerTurnAction;
//     public event Action TurnUpdate;
//     public Weapon weapon;
//     // public event Action PlayerActionChosen;

//     //TurnHolder 0 = PreTurn, TurnHolder 1 = player, TurnHolder 2+ = enemies

//     public bool waitingForPlayerInput = false;
//     bool canActiveAttack = false;
//     bool activeAttackLockout = true;
//     bool activeAttackSuccess = false;
//     bool doingQTE = false;
    


//     void Start()
//     {
//         TurnUpdate += UpdateTurnDisplay;
//     }

//     public void changeWeapon(Weapon newWeapon)
//     {
//         weapon = newWeapon;
//     }

//     public void UpdateTurnDisplay()
//     {
//         turnTrackerText.text = "Turn: " + turnHolder;
//     }

//     public void NextTurn(int skipCount = 1)
//     {
//         turnHolder += skipCount;
//         TurnUpdate.Invoke();
//     }

//     public void StartBattle()
//     {
//         turnHolder = TurnHolder.PreTurn;
//         StartTurn();
//     }

//     void StartTurn()
//     {
//         switch((int)turnHolder)
//         {
//             case 0:
//             NextTurn();
//             StartTurn();
//             break;

//             case 1:
//             waitingForPlayerInput = true;
//             StartCoroutine(WaitForPlayerAction());
//             break;

//             case 2:
//             break;

//             default:
//             break;
//         }

//     }

//     IEnumerator WaitForPlayerAction()
//     {
//         yield return new WaitUntil(()=> waitingForPlayerInput == false);
//         switch((int)playerTurnAction)
//         {
//             case 0:
//             break;

//             case 1:
//             StartCoroutine(PlayerAttackQTE(weapon));
//             yield return new WaitUntil(()=> doingQTE == false);
            
//             // Target.DamageEnemyHealth(PlayerAttack(weapon)); //supposed to deal damage to targetted entity
//             break;

//             case 2:
//             break;

//             case 3:
//             break;

//             case 4:
//             break;

//             default:
//             break;
//         }

//     }

//     public int PlayerAttack(Weapon weapon, bool isSpecialAttack = false)
//     {
//         int damage = 1;

//         if (!isSpecialAttack)
//         {
//             if (activeAttackSuccess)
//             {
//                 damage = weapon.damageBonus;
//             }
//             else
//             damage = weapon.damage;
//         }
//         if (!isSpecialAttack)
//         {
//             damage = weapon.specialDamage;
//         }




//         return damage;
//     }

//     IEnumerator PlayerAttackQTE(Weapon weapon, bool isSpecialAttack = false)
//     {
//         canActiveAttack = false;
//         if (!isSpecialAttack)
//         {
//             activeAttackLockout = false;
//             yield return new WaitForSeconds(weapon.attackTime);
//             activeAttackSuccess = false;
//             canActiveAttack = true;
//             doingQTE = true;
//             StartCoroutine(SweetspotTime(.5f));
//             StartCoroutine(SweetspotMisstime(.5f));
//             yield return new WaitUntil(()=> doingQTE = false);
//         }
//         else if (isSpecialAttack)
//         {
//             yield return new WaitForSeconds(weapon.specialAttackTime);
//         }
        
//     }

//     IEnumerator SweetspotTime(float waitTime)
//     {
//         yield return new WaitForSeconds(waitTime);
//         float timer = 0.0f;
//         while (!activeAttackLockout && canActiveAttack)
//         {
//             if (Input.GetKeyDown(KeyCode.F))
//             {
//                 activeAttackSuccess = true;
//                 activeAttackLockout = true;
//             }
//             else if (timer >= .3f)
//             {
//                 activeAttackLockout = true;
//             }
//             timer += Time.deltaTime;
//         }
//         doingQTE = false;
//     }

//     IEnumerator SweetspotMisstime (float waitTime)
//     {
//         float timer = 0.0f;
//         yield return new WaitForSeconds(waitTime-.2f);
//         bool canMisstime = true;
//         while (canMisstime)
//         {
//             if (Input.GetKeyDown(KeyCode.F))
//             {
//                 activeAttackLockout = true;
//             }
//             else if (timer >= .2f)
//             {
//                 canMisstime = false;
//             }
//             timer += Time.deltaTime;
//         }
//     }

// }
