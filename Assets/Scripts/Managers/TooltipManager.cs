using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    [Header("Entity Tip")]
    public GameObject SmallEntityTipBox;
    public GameObject BigEntityTipBox;
    public TMP_Text entityName;
    public TMP_Text entityHealth;
    public TMP_Text entityPower;
    public TMP_Text entityPowerShort;
    public TMP_Text entityDefense;
    public TMP_Text entityDefenseShort;
    public TMP_Text entityFlavor;
    public UnityEngine.UI.Image entitySprite;
    private Entity currentEntity;
    public GameObject dropDown;
    public bool expanded = false;

    [Header("WeaponTip")]
    public GameObject BLAnchor;
    public GameObject weaponTipPanel;
    public TMP_Text weaponName;
    public TMP_Text weaponPower;
    public TMP_Text weaponYield;
    public TMP_Text weaponCost;
    public TMP_Text weaponDurability;
    public TMP_Text weaponDesc;
    private WeaponInventoryContainer currentWeapon;
    public bool showingWeaponTip = false;



    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TurnManager.instance.OnUpdateHealthStats += UpdateEntityTip;
        BattleManager.instance.OnEnemyKilled += DontShowDeadEnemy;
    }

    public void EntityStatTip(Entity e)
    {
        currentEntity = e;
        entityName.text = e.entityType.displayName;
        entityHealth.text = e.currentHealth + "/" + e.maxHealth;
        float PowCalc = (e.power+e.powerFlatMod)*e.powerMultMod;
        entityPower.text = "(" + e.power + " + " + e.powerFlatMod + ") * " + e.powerMultMod + " = " + PowCalc;
        entityPowerShort.text = PowCalc.ToString();
        float DefCalc = (e.defense+e.defenseFlatMod)*e.defenseMultMod;
        entityDefense.text = "(" + e.defense + " + " + e.defenseFlatMod + ") * " + e.defenseMultMod + " = " + DefCalc;
        entityDefenseShort.text = DefCalc.ToString();

        entitySprite.sprite = e.entityType.sprite;
        entityFlavor.text = e.entityType.flavor;
    }

    public void UpdateEntityTip()
    {
        EntityStatTip(currentEntity);
    }

    // public void UpdateEntityTip()
    // {

    // }

    // public void ShowEntityTip()
    // {
    //     if (!showingTip)
    //     {
    //         SmallEntityTipBox.SetActive(true);
    //         showingTip = true;
    //     }
    // }

    // public void HideEntityTip()
    // {
    //     if (showingTip)
    //     {
    //         EntityTipBox.SetActive(false);
    //         showingTip = false;
    //     }
    // }

    public void ToggleEntityTip()
    {
        if (expanded)
        {
            CompressEntityTip();
        }
        else
        {
            ExpandEntityTip();
        }
    }
    public void ExpandEntityTip()
    {
        if (!expanded)
        {
            SmallEntityTipBox.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,255,255,0);
            dropDown.SetActive(false);
            BigEntityTipBox.SetActive(true);
            expanded = true;
        }
        
    }

    public void CompressEntityTip()
    {
        if (expanded)
        {
            SmallEntityTipBox.GetComponent<UnityEngine.UI.Image>().color = new Color32(255,255,255,255);
            dropDown.SetActive(true);
            BigEntityTipBox.SetActive(false);
            expanded = false;
        }
        
    }

    public void ShowAndCreateEntityTip(Entity e)
    {
        EntityStatTip(e);
        // ShowEntityTip();
    }

    private void DontShowDeadEnemy()
    {
        Debug.LogWarning("picked up on OnEnemyKilled");
        if (BattleManager.instance.FindEnemyInSlot(currentEntity) > BattleManager.instance.enemyBattlePositions.Length)
        {
            EntityStatTip(BattleManager.instance.playerObject.GetComponent<Entity>());
        }
    }

    void Update()
    {
        // if (showingTip) tooltip mouse follow
        // {
        //     EntityTipBox.transform.position = Input.mousePosition;
        // }
        if (showingWeaponTip)
        {
            BLAnchor.transform.position = Input.mousePosition;
        }
    }

    public void WeaponStatTip(WeaponInventoryContainer weap)
    {
        currentWeapon = weap;
        weaponName.text = weap.weapon.itemName;
        weaponYield.text = weap.weapon.sugarYield.ToString();
        weaponCost.text = weap.weapon.attackDurabilityCost.ToString();
        weaponPower.text = weap.weapon.damage.ToString();
        weaponDurability.text = weap.durability + "/" + weap.weapon.maxDurability;
        weaponDesc.text = weap.weapon.description;
    }

    public bool SetWeaponStatTip(int slot)
    {
        if (slot < 0)
        {
            WeaponInventoryContainer defWeap = new WeaponInventoryContainer(BattleManager.instance.defaultWeapon, 1);
            WeaponStatTip(defWeap);
            return true;
        }
        else if (WeaponInventory.instance.weapons[slot] != null)
        {
            WeaponStatTip(WeaponInventory.instance.weapons[slot]);
            return true;
        }
        return false;
    }

    public void ShowWeaponTip(int slot)
    {
        if (!showingWeaponTip)
        {
            if (SetWeaponStatTip(slot))
            {
                weaponTipPanel.SetActive(true);
                showingWeaponTip = true;
            }
        }
    }

    public void HideWeaponTip()
    {
        if (showingWeaponTip)
        {
            weaponTipPanel.SetActive(false);
            showingWeaponTip = false;
        }
    }

}
