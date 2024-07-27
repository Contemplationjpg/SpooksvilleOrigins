using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private bool CanPickUp;
    public Weapon weapon;
    public int durability = 0;
    public bool IsInOverworld = true;
    public bool PickedUp = false;
    public bool Infinite = false;


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
            PickUpWeapon();
            }
        }
    }

    public void PickUpWeapon()
    {
        if (!PickedUp)
        {
            Debug.Log("Picking up: " + weapon.itemName);
            bool wasPickedUp = WeaponInventory.instance.AddWeapon(weapon, durability);
            Debug.Log("Picked up: " + wasPickedUp);

            if (wasPickedUp)
            {
                if (Infinite)
                {
                    PickedUp = false;
                }

                else if (IsInOverworld)
                {
                    PickedUp = true;
                    Destroy(gameObject);
                }
                else
                {
                    PickedUp = true;
                }
            }
            else
            Debug.Log("inventory full, did not pick up weapon " + weapon.itemName);
        }
        else
        {
            Debug.Log("No more of weapon, "  + weapon.itemName + ", to pick up ");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("YIPPEE");
            CanPickUp = true;
            //Debug.Log("CanPickUp = true");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("NOT YIPPEE");
            CanPickUp = false;
            //Debug.Log("CanPickUp = false");
        }
    }


}
