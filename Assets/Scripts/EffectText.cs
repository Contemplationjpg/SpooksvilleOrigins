using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class EffectText : MonoBehaviour
{
    public String text;
    public TMP_Text textBox;
    private float secondsToDestroy = 1f;

    void Start()
    {
        Destroy(gameObject, secondsToDestroy);
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x+0.001f, transform.position.y+0.001f);
    }

    public void SetString(String txt)
    {
        text = txt;
        textBox.text = text;
    }
}
