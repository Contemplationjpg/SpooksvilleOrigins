using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName = "New Item";
    public string displayName = "New Item";
    public Sprite icon = null;
    public string description = "";
    public bool isDefaultItem = false;
    public bool isStackable = false;
    public int maxStackSize = 99;

    [Header("Healing")]
    public bool isHealItem = false;
    public int healAmount = 0;
    [Header("Sugar")]
    public bool isSugarItem = false;
    public int sugarAmount = 0;
    [Header("Buffs")]
    public bool isBuffItem = false;
    public bool buffsPower = false;
    public int powerBuff = 0;
    public float powerMult = 0;
    public bool buffsDefense = false;
    public int defenseBuff = 0;
    public float defenseMult = 0;
    [Header("Misc")]
    public bool hasDefensiveProperty = false;
    public float defensiveProperty = 0f;



    public virtual void Use()
    {
        if (TurnManager.instance.state == TurnManager.State.WaitingForPlayerInput)
        {
            Debug.Log("Using " + itemName);
            TurnManager.instance.selectedItem = this;
            TurnManager.instance.choice = TurnManager.Choice.UseItem;
            TurnManager.instance.ChoiceChosen = true;
        }
        
        
    }

    public virtual bool AttemptItemUse()
    {
        if (Inventory.instance.RemoveItem(this, 1))
        {
            if (UseItem())
            {
                Debug.Log("Used item: " + itemName);
                return true;
            }
            Debug.Log("Could not remove item: " + itemName);
            return false;
            
            
            
        }
        else
        {
            return false;
        }
    }

    private bool UseItem()
    {
        bool itemUsed = false;
        if (isHealItem)
        {
            Debug.Log("Healing player for " + healAmount);
            int healedHealth = BattleManager.instance.playerHealth.IncreaseHealth(healAmount);
            BattleManager.instance.SpawnEffectText(healedHealth.ToString(),-1,"green");
            itemUsed = true;
        }
        if (isSugarItem)
        {
            Debug.Log("Increasing player sugar for " + sugarAmount);
            int gainedSugar = BattleManager.instance.playerSugar.IncreaseSugar(sugarAmount);
            BattleManager.instance.SpawnEffectText(gainedSugar.ToString(),-1,"white");
            itemUsed = true;
        }
        if (isBuffItem)
        {
            if (buffsPower)
            {
                Debug.Log("Buffing power by flat " + powerBuff + " and mult " + powerMult);
                if (powerBuff!=0)
                {
                    BattleManager.instance.playerObject.GetComponent<Player>().powerFlatMod += powerBuff;
                    BattleManager.instance.SpawnEffectText("+"+powerBuff.ToString(),-1,"red");
                }
                if (powerMult!=0)
                {
                    BattleManager.instance.playerObject.GetComponent<Player>().powerMultMod += powerMult;
                    BattleManager.instance.SpawnEffectText("+x"+powerMult.ToString(),-1,"red");
                }
                
                itemUsed = true;
            }
            if (buffsDefense)
            {
                Debug.Log("Buffing defense by flat " + defenseBuff + " and mult " + defenseMult);
                if (defenseBuff!=0)
                {
                    BattleManager.instance.playerObject.GetComponent<Player>().defenseFlatMod += defenseBuff;
                    BattleManager.instance.SpawnEffectText("+"+defenseBuff.ToString(),-1,"blue");
                }
                if (defenseMult!=0)
                {
                    BattleManager.instance.playerObject.GetComponent<Player>().defenseMultMod += defenseMult;
                    BattleManager.instance.SpawnEffectText("+x"+defenseMult.ToString(),-1,"blue");
                }
                itemUsed = true;
            }
        }
        if (!itemUsed)
        {
            Debug.LogWarning("Item: " + itemName + " does nothing");
        }
        return itemUsed;
    }


}
