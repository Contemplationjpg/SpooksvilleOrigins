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
        playerHealthBar.ReduceHealth(battleManager.CalculateDamage(weapon, enemy, player));
    }

    public void EnemyHeal()
    {
        battleManager.enemyHealthBars[battleManager.FindEnemyInSlot(enemy)].IncreaseHealth(20);
    }

    public void EnemyBuff(int pow = 0, int def = 0)
    {
        enemy.powerFlatMod+=pow;
        enemy.defenseFlatMod+=def;
    }





}
