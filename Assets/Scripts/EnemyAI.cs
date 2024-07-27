using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public GameObject playerGO;
    public HealthBar playerHealthBar;
    public Entity enemy;
    public Weapon weapon;

    private BattleManager battleManager;
    public Player player;
    

    private int attackDelay;
    public int attackTimer = 1;


    void Awake()
    {
        battleManager = BattleManager.instance;
    }

    public void SetUp()
    {
        attackDelay = enemy.attackDelay;
    }

    public void EnemyDecision()
    {
        if(attackTimer == attackDelay)
        {
            EnemyAttack();
            attackTimer = 0;
        }
        else
        attackTimer++;
    }

    public void EnemyAttack()
    {
        playerHealthBar.ReduceHealth(battleManager.CalculateDamage(weapon, enemy, player));
    }





}
