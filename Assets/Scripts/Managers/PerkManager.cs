using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public bool GivePerk(String perkName)
    {
        if (perkName.Equals("SugarSteal"))
        {
            if (!BattleManager.instance.player.sugarSteal)
            {
                BattleManager.instance.player.sugarSteal = true;
                return true;
            }
            else
            return false;
        }
        else if (perkName.Equals("PassBlock"))
        {
            if (!BattleManager.instance.player.passBlock)
            {
                BattleManager.instance.player.passBlock = true;
                return true;
            }
            else
            return false;
        }
        return false;
    }

    
    
}
