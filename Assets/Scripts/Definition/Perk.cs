using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/Perk")]
public class Perk : ScriptableObject
{
    public string perkName = "default name";
    public string displayName = "New Item";
    public Sprite sprite = null;
    public string description = "";
}
