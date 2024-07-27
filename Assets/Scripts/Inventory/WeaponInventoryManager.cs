using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryManager : MonoBehaviour
{
    public GameObject[] weaponSlots;
    Image[] slotIcons;
    TMP_Text[] durabilityTextboxes;
    Button[] buttons;
    WeaponInventory weaponInventory;

    void Awake()
    {
        slotIcons = new Image[weaponSlots.Length];
        durabilityTextboxes = new TMP_Text[weaponSlots.Length];
        buttons = new Button[weaponSlots.Length];

        for(int i = 0; i < weaponSlots.Length; i++)
        {
            slotIcons[i] = weaponSlots[i].GetComponent<Image>();
            durabilityTextboxes[i] = weaponSlots[i].GetComponentInChildren<TMP_Text>();
            buttons[i] = weaponSlots[i].GetComponent<Button>();


            slotIcons[i].color = new Color32(255,255,255,0);
            durabilityTextboxes[i].text = "";
            buttons[i].interactable = false;
        }
    
    }

    void Start()
    {
        weaponInventory = WeaponInventory.instance;
        weaponInventory.OnWeaponChangedCallBack += UpdateWeaponUI;
    }

    public void ResetWeaponUI()
    {
        for(int i = 0; i < weaponSlots.Length; i++)
        {   
            if (weaponInventory.weapons[i] !=null)
            {
                slotIcons[i].color = new Color32(255,255,255,0);
                durabilityTextboxes[i].text = "";
                buttons[i].interactable = false;
            }
        }
    }

    void UpdateWeaponUI()
    {
        ResetWeaponUI();
            for (int i = 0; i < weaponInventory.weapons.Length; i++)
            {
                if (weaponInventory.weapons[i] != null)
                {
                    slotIcons[i].color = new Color32(255,255,255,255);
                    slotIcons[i].sprite = weaponInventory.weapons[i].weapon.icon;
                    durabilityTextboxes[i].text = weaponInventory.weapons[i].durability.ToString();
                    buttons[i].interactable = true;
                    WeaponInventoryContainer thisWeapon = weaponInventory.weapons[i];
                    int currentInventorySlot = i;
                    buttons[i].onClick.AddListener(() => BattleManager.instance.SelectNewWeapon(currentInventorySlot));
                }
            }
    }



}
