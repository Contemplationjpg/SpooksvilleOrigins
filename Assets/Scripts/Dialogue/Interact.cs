using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Dialogue dia;
    private bool canChat;
    // public Collider2D interactBox;
    // Start is called before the first frame update
    void Start()
    {
        canChat = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (canChat)
       { 
            dia.DialogueButton();
       }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("YIPPEE");
            canChat = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("NOT YIPPEE");
            canChat = false;
            // dia.EndText();
        }
    }


}
