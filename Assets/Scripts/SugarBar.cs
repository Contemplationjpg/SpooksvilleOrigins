using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SugarBar : MonoBehaviour
{

    public Player entity;
    public GameObject sugarBarGO;
    private Slider sugarBar;
    private TMP_Text sugarText;
    
    

    public void Awake()
    {
        InitializeSugarBar();
        HideSugarBar();
    
    }

    public void InitializeSugarBar()
    {
        sugarBar = sugarBarGO.GetComponent<Slider>();
        sugarText = sugarBarGO.GetComponentInChildren<TMP_Text>();

        sugarBar.minValue = 0;
    }

    public void HideSugarBar()
    {
        // Debug.Log("setting sugarbar to deactive");
        sugarBarGO.SetActive(false);
    }

    public void ShowSugarBar()
    {
        // Debug.Log("setting sugarbar to active");
        sugarBarGO.SetActive(true);
    }

    public void SetSugar(int newSugar)
    {
        if (newSugar<=entity.maxSugar)
        {
            entity.currentSugar = newSugar;
            UpdateSugarBar();
        }
        else
        {
            entity.currentSugar=entity.maxSugar;
            UpdateSugarBar();
        }
        

    }

    public int ReduceSugar(int reduceSugar)
    {
        int lostSugar = 0;
        if (sugarBar.value - reduceSugar < 0)
        {
            lostSugar = entity.currentSugar;
            entity.currentSugar = 0;
            UpdateSugarBar();
            return lostSugar;
        }
        else
        {
            lostSugar = reduceSugar;
            entity.currentSugar-=reduceSugar;
            UpdateSugarBar();
            return lostSugar;
        }
    }

    public int IncreaseSugar(int increaseSugar)
    {
        int gainedSugar = 0;
        if (increaseSugar<0)
        increaseSugar=0;
        if(entity.currentSugar+increaseSugar<=entity.maxSugar)
        {
            gainedSugar = increaseSugar;
            entity.currentSugar+=increaseSugar;
            UpdateSugarBar();
            return gainedSugar;
        }
        else
        {
            gainedSugar = entity.maxSugar - entity.currentSugar;
            entity.currentSugar=entity.maxSugar;
            UpdateSugarBar();
            return gainedSugar;
        }
    }

    public int GetSugar()
    {
        return entity.currentSugar;
    }

    private void UpdateText()
    {
        string newSugarText = sugarBar.value.ToString() + "/" + entity.maxSugar;
        sugarText.text = newSugarText;
    }
    
    public void UpdateSugarBar()
    {
        if (entity != null)
        {
        sugarBar.maxValue = entity.maxSugar;
        sugarBar.value = entity.currentSugar;
        UpdateText();    
        }
        else
        {
            Debug.Log("No Entity Set For SugarBar");
        }
    }

}
