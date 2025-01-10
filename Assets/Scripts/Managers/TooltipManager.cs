using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
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
    }

}
