using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Entity entity;
    public GameObject healthBarGO;
    private Slider healthBar;
    private TMP_Text healthText;
    
    

    public void Awake()
    {
        InitializeHealthBar();
        HideHealthBar();
    
    }

    public void InitializeHealthBar()
    {
        healthBar = healthBarGO.GetComponent<Slider>();
        healthText = healthBarGO.GetComponentInChildren<TMP_Text>();

        healthBar.minValue = 0;
    }

    public void HideHealthBar()
    {
        Debug.Log("setting healthbar to deactive");
        healthBarGO.SetActive(false);
    }

    public void ShowHealthBar()
    {
        Debug.Log("setting healthbar to active");
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
        if(entity.currentHealth+increaseHealth<=entity.maxHealth)
        {
            entity.currentHealth+=increaseHealth;
            UpdateHealthBar();
        }
        else
        {
            entity.currentHealth=entity.maxHealth;
            UpdateHealthBar();
        }
    }

    public int GetHealth()
    {
        return entity.currentHealth;
    }

    private void UpdateText()
    {
        string newHealthText = healthBar.value.ToString() + "/" + entity.maxHealth;
        healthText.text = newHealthText;
    }
    
    public void UpdateHealthBar()
    {
        if (entity != null)
        {
        healthBar.maxValue = entity.maxHealth;
        healthBar.value = entity.currentHealth;
        UpdateText();    
        }
        else
        {
            Debug.Log("No Entity Set For HealthBar");
        }
    }

}
