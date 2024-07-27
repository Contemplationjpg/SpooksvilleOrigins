using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    Rigidbody2D rb;
    float horizontal = 0;
    float vertical = 0;
    float speedLimiter = 0.707106f;
    public static bool playerMovementLocked;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovementLocked = false;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (playerMovementLocked || Dialogue.dialogueRunning)
        {
            rb.velocity = new Vector2(0,0);
        }
        else
        {
            if(horizontal != 0 && vertical != 0)
                {
                rb.velocity = new Vector2(horizontal*movementSpeed*speedLimiter*Time.timeScale, vertical*movementSpeed*speedLimiter*Time.timeScale);
                }
            else
                rb.velocity = new Vector2(horizontal*movementSpeed*Time.timeScale, vertical*movementSpeed*Time.timeScale);
        }
    }
}
