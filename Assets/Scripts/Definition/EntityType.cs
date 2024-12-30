using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Entity/Entity")]
public class EntityType : ScriptableObject
{
    public string entityName = "default name";
    public string displayName = "New Item";
    public Sprite sprite = null;
    public bool isEnemy = true;
    public EnemyAI ai;
    public Weapon weapon = null;

    //stats
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int defense = 10;
    public int power = 10;
    public string weaknessTag = "";
    public float luck;
    public bool canCrit = true;
    public int attackDelay = 0;
}
