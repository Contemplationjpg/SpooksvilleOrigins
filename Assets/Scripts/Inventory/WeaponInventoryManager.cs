using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryManager : MonoBehaviour
{
    public SpriteRenderer selectedWeapon;
    public GameObject[] weaponSlots;
    Image[] slotIcons;
    TMP_Text[] durabilityTextboxes;
    Button[] buttons;
    WeaponInventory weaponInventory;
    public static bool weaponInventoryInitialized = false;
    public Image defaultSlot;
    // Image defaultSlotIcon;
    // Button defaultButton;



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

        // defaultSlotIcon = defaultSlot.GetComponent<Image>();
        // defaultButton = defaultSlot.GetComponent<Button>();

        // defaultSlotIcon.color = new Color32(255,255,255,0);
        // defaultButton.interactable = true;

        InitializeDefaultWeapon();
        
    }

    void Start()
    {
        weaponInventory = WeaponInventory.instance;
        weaponInventory.OnWeaponChangedCallBack += UpdateWeaponUI;
        BattleManager.instance.OnWeaponSelectedCallback += UpdateSelectedWeaponUI;
        BattleManager.instance.OnDefaultWeaponChanged += UpdateDefaultWeaponUI;
        UpdateDefaultWeaponUI();
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
                try 
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
                catch(Exception ex)
                {
                    Debug.LogWarning("COCK");
                    Debug.LogException(ex);
                    return;
                }
                
            }
    }

    void UpdateDefaultWeaponUI()
    {
        // defaultSlotIcon.color = new Color32(255,255,255,255);
        defaultSlot.sprite = BattleManager.instance.defaultWeapon.icon;
    }

    void UpdateSelectedWeaponUI()
    {
        // Debug.Log("Updating Selected Weapon UI");
        if (BattleManager.instance.newSelectedWeaponSlot >= 0)
        {
            // selectedWeapon.color = new Color32(0,0,0,0);
            selectedWeapon.sprite = weaponInventory.weapons[BattleManager.instance.newSelectedWeaponSlot].weapon.icon;    
        }
        
        else
        {
            // selectedWeapon.color = new Color32(255,255,255,255);
            selectedWeapon.sprite = BattleManager.instance.defaultWeapon.icon;
        }
        
    }

    IEnumerator InitializeDefaultWeapon()
    {
        yield return new WaitUntil(()=>BattleManager.playerInitialized = true);
        Debug.Log("Initializing Default Weapon");
        UpdateDefaultWeaponUI();
    }




}
