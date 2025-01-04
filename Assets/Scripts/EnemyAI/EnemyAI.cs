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
        anim.DoLeftSlide();
        EnemyAttack();
    }

    public void EnemyAttack()
    {
        playerHealthBar.ReduceHealth(battleManager.CalculateDamage(weapon, enemy, player));
    }





}
