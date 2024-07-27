using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.1f;
    public string[] lines;

    private int index = 0;
    public static bool dialogueRunning = false;
    bool myDialogueRunning = false;
    

    void Start()
    {
        textComponent.text = string.Empty;
        dialogueRunning = false;
        Debug.Log("dialogueRunning = false");
    }

    void Update()
    {
    }

    public void DialogueButton()
    {
        if (Input.GetKeyDown(KeyCode.F)&&!GameManager.GamePaused&&!GameManager.InventoryOpen)
        {
            if (dialogueRunning&&myDialogueRunning)
            {
                if (textComponent.text == lines[index]+"<color=#0000>")
                {
                NextLine();
                }
                else
                {
                StopAllCoroutines();
                textComponent.text = lines[index]+"<color=#0000>";
                }
            }
            else
            if(!dialogueRunning)
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        if (!dialogueRunning)
        {
            index = 0;
            StartCoroutine(TypeLine());
            dialogueRunning = true;
            myDialogueRunning = true;
            Debug.Log("dialogueRunning = true");
        }
    }

    IEnumerator TypeLine()
    {
        int tempIndex = 0;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text = lines[index].Insert(tempIndex,"<color=#0000>");
            // textComponent.text += c;
            tempIndex+=1;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
        textComponent.text = lines[index]+"<color=#0000>";
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("dialogueRunning = false");
            textComponent.text = string.Empty;
            index = 0;
            dialogueRunning = false;
            myDialogueRunning = false;
        }
    }

    // public void EndText()
    // {
    //     StopAllCoroutines();
    //     dialogueRunning = false;
    //     textComponent.text = string.Empty;
    //     index = 0;
    //     PlayerMovement.playerMovementLocked = false;
    //     Debug.Log("playermovementlocked = false");
    // }

}
