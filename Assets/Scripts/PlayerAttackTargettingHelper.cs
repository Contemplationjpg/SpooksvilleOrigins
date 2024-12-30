using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerAttackTargettingHelper : MonoBehaviour
{
    public GameObject arrow;
    private GameObject CurrentTarget;
    public static bool checkingForMouse;
    public static PlayerAttackTargettingHelper instance;
    public List<Entity> targets;
    public bool doingSpecial = false;

    void Awake()
    {
        instance = this;
        targets.Clear();
    }
    void Start()
    {
        checkingForMouse = false;
        UpdateVisibility();
    }

    void Update()
    {

    }

    public void AttemptToAddTargetToList(Entity newTarget)
    {
        if (BattleManager.instance.newSelectedWeaponSlot<=WeaponInventory.instance.weapons.Length && newTarget != null)
        {
            if (targets.Count < WeaponInventory.instance.weapons[BattleManager.instance.newSelectedWeaponSlot].weapon.numberOfTargets)
            {
                if (targets.Count>0)
                {
                    foreach (Entity e in targets)
                    {
                        if (e == newTarget)
                        {
                            Debug.Log("Entity, " + newTarget.entityName + ", already in list of targets.");
                            return;
                        }
                    }
                }
                if (newTarget.isEnemy)
                {
                        Debug.Log("Adding entity, " + newTarget.entityName + ", to list of targets.");
                        targets.Add(newTarget);
                }
            }
            
        }
    }

    public void ClearTargetList()
    {
        targets.Clear();
    }

    public void ChangeCheckBool(bool checkingbool)
    {
        if (checkingForMouse!=checkingbool)
        {
        checkingForMouse = checkingbool;  
        UpdateVisibility();
        }
        

    }

    public void ToggleBool()
    {
        checkingForMouse = !checkingForMouse;
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        if (!checkingForMouse)
        {
            HideArrow();
        }
        else
        {
            arrow.transform.position = new Vector2(1000,1000);
            ShowArrow();
        }
    }

    public GameObject GetTarget()
    {
        return CurrentTarget;
    }

    public void HideArrow()
    {
        if (arrow.activeSelf)
        {
            arrow.SetActive(false);
        }
    }
    
    public void ShowArrow()
    {
        if (!arrow.activeSelf)
        {
            arrow.SetActive(true);
        }
    }

    public void UpdateArrowPosition(GameObject newGameObject)
    {
        CurrentTarget = newGameObject;
        SpriteRenderer spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
        arrow.transform.position = new Vector2(newGameObject.transform.position.x, newGameObject.transform.position.y+(spriteRenderer.size.y/2*newGameObject.transform.localScale.y)+.5f);
        Debug.Log("Moved Arrow to: (" + arrow.transform.position.x + ", " + arrow.transform.position.y + ").");
    }

    public void DoingSpecial(bool doing)
    {
        doingSpecial = doing;
    }


}
