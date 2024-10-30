using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : MonoBehaviour
{
    private bool CanPickUp;
    public Item item;
    public int amount = 1;
    public bool IsInOverworld;
    public bool PickedUp = false;


    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Overworld")
        {
            IsInOverworld = true;
        }
        else 
        IsInOverworld = false;
    }
    private void Start()
    {
        CanPickUp = false;
    }
    private void Update()
    {
        if (CanPickUp)
        {
            if (Input.GetKeyDown(KeyCode.F)&&!GameManager.GamePaused&&IsInOverworld)
            {
            PickUpItem();
            }
        }
    }

    public void PickUpItem()
    {
        if (!PickedUp)
        {
            Debug.Log("Picking up: " + item.name);
                int wasPickedUp = Inventory.instance.AddItem(item, amount);
                Debug.Log("Picked up: " + wasPickedUp);
                if (wasPickedUp<=amount)
                {
                    amount -= wasPickedUp;
                }

                if (amount==0||amount<0)
                {
                    if (IsInOverworld)
                    {
                        Destroy(gameObject);
                    }
                        PickedUp = true;
                }
                else
                    Debug.Log("remaining amount: " + amount);
        }
                
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&IsInOverworld)
        {
            //Debug.Log("YIPPEE");
            CanPickUp = true;
            //Debug.Log("CanPickUp = true");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&IsInOverworld)
        {
            //Debug.Log("NOT YIPPEE");
            CanPickUp = false;
            //Debug.Log("CanPickUp = false");
        }
    }


}
