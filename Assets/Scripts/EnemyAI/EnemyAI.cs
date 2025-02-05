using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public GameObject playerGO;
    public HealthBar playerHealthBar;
    public Entity enemy;
    public Weapon weapon;

    private BattleManager battleManager;
    public Player player;



    void Awake()
    {
        battleManager = BattleManager.instance;
    }

    public void SetUp()
    {
        
    }

    public void EnemyDecision()
    {
        SimpleAnimation anim = GetComponent<SimpleAnimation>();
        int percent = (enemy.currentHealth*100)/enemy.maxHealth;
        Debug.Log("Enemy health is at " + percent + " percent");
        if (percent < 50)
        {
            anim.DoLittleHop();
            EnemyHeal();
            EnemyBuff(0,20);
        }
        else
        {
            anim.DoLeftSlide();
            EnemyAttack();
        }
        
    }

    public void EnemyAttack()
    {
        int damage = battleManager.CalculateDamage(weapon, enemy, player);
        if (TurnManager.instance.playerDefending)
        {
            damage = (int)Math.Round(damage * TurnManager.instance.playerDefendingMod);  
        }
        int dealtDamage = playerHealthBar.ReduceHealth(damage);
        battleManager.SpawnEffectText(dealtDamage.ToString(),-1,"white");
    }

    public void EnemyHeal()
    {
        int slot = battleManager.FindEnemyInSlot(enemy);
        int healedHealth = battleManager.enemyHealthBars[slot].IncreaseHealth(20);
        battleManager.SpawnEffectText(healedHealth.ToString(),slot,"green");
    }

    public void EnemyBuff(int pow = 0, int def = 0)
    {
        int slot = battleManager.FindEnemyInSlot(enemy);
        if (pow!=0)
        {
            enemy.powerFlatMod+=pow;
            battleManager.SpawnEffectText("+"+pow.ToString(),slot,"red");
        }
        if (def!=0)
        {
            enemy.defenseFlatMod+=def;
            battleManager.SpawnEffectText("+"+def.ToString(),slot,"blue");
        }
    }





}
