using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
// [System.Serializable]
public class Weapon : Item
{
    public int damage = 10;
    public int damageBonus = 13;
    public int specialDamage = 20;
    public int specialDamageBonus = 25;
    public int critChanceBoost = 0;
    public bool canCrit = true;
    public float critMult = 1.5f;
    public string damageType = "";
    public int maxDurability = 30;
    public int numberOfTargets = 1;


    public float attackTime = 1f;
    public float specialAttackTime = 1f;

}
