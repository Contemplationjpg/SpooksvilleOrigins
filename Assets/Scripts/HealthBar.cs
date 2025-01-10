using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public bool isEnemy = true;

    public Entity entity;
    public GameObject healthBarGO;
    private Slider healthBar;
    private TMP_Text healthText;
    private SpriteRenderer itemSR;
    [SerializeField]
    private bool canOverheal = false;
    private bool hasOverhealLimit = false;
    private int overhealLimit = 0;
    

    public void Awake()
    {
        InitializeHealthBar();
        HideHealthBar();
    
    }

    public void InitializeHealthBar()
    {
        healthBar = healthBarGO.GetComponent<Slider>();
        healthText = healthBarGO.GetComponentInChildren<TMP_Text>();
        if (isEnemy)
        {
            itemSR = healthBarGO.GetComponentInChildren<SpriteRenderer>();
        }
        healthBar.minValue = 0;
    }

    public void HideHealthBar()
    {
        // Debug.Log("setting healthbar to deactive");
        healthBarGO.SetActive(false);
    }

    public void ShowHealthBar()
    {
        // Debug.Log("setting healthbar to active");
        healthBarGO.SetActive(true);
    }

    public void SetHealth(int newHealth)
    {
        if (newHealth<=entity.maxHealth)
        {
            entity.currentHealth = newHealth;
            UpdateHealthBar();
        }
        else
        {
            entity.currentHealth=entity.maxHealth;
            UpdateHealthBar();
        }
        

    }

    public void ReduceHealth(int reduceHealth)
    {
        if (healthBar.value - reduceHealth < 0)
        {
            entity.currentHealth = 0;
            UpdateHealthBar();
        }
        else
        {
            entity.currentHealth-=reduceHealth;
            UpdateHealthBar();
        }
    }

    public void IncreaseHealth(int increaseHealth)
    {
        if (increaseHealth<0)
        increaseHealth=0;
        int newHealth = entity.currentHealth+increaseHealth;
        if (canOverheal)
        {
            if (hasOverhealLimit)
            {
                int limit = entity.maxHealth;
                limit += overhealLimit;
                if (newHealth<=limit)
                {
                    entity.currentHealth = newHealth;
                    UpdateHealthBar();
                    return;
                }
                else
                {
                    entity.currentHealth = limit;
                    UpdateHealthBar();
                    return;
                }
            }
            entity.currentHealth+=increaseHealth;
            UpdateHealthBar();
            return;
        }
        if(newHealth > entity.maxHealth)
        {
            if (entity.currentHealth > entity.maxHealth)
            {
                UpdateHealthBar();
                return;
            }
            else
            {
                entity.currentHealth=entity.maxHealth;
                UpdateHealthBar();
                return;
            }
        }
        else
        {
            entity.currentHealth=newHealth;
            UpdateHealthBar();
        }
    }

    public void ChangeEntity(Entity e)
    {
        entity = e;
        UpdateHealthBar();
    }

    public int GetHealth()
    {
        return entity.currentHealth;
    }

    private void UpdateText()
    {
        string newHealthText = entity.currentHealth.ToString() + "/" + entity.maxHealth;
        healthText.text = newHealthText;
    }

    public void SetWeapon()
    {
        if (isEnemy)
        {
            itemSR.sprite = entity.entityType.weapon.icon;    
        }
        
    }
    
    public void UpdateHealthBar()
    {
        if (entity != null)
        {
        healthBar.maxValue = entity.maxHealth;
        healthBar.value = entity.currentHealth;
        canOverheal = entity.canOverheal;
        hasOverhealLimit = entity.hasOverhealLimit;
        overhealLimit = entity.overhealLimit;
        UpdateText();    
        }
        else
        {
            Debug.Log("No Entity Set For HealthBar");
        }
    }

    public void InitializeSpriteRenderer()
    {
        
    }

}
