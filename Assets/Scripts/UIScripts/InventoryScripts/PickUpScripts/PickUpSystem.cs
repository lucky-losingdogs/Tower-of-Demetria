using UnityEngine;
using System;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //assigning the pick up the player has collided with to a var
        ItemPickUp itemPickUp = collision.GetComponent<ItemPickUp>();
        
        if (itemPickUp != null)
        {
            AddPickUp(itemPickUp);
        }
    }

    public void AddPickUp(ItemPickUp itemPickUp)
    {
        //add the item to InventorySO
        //if inventory is full, the remainder is returned here
        int remainderQuantity = inventoryData.AddItem(itemPickUp.m_item, itemPickUp.m_quantity);

        if (remainderQuantity == 0)
            itemPickUp.DestroyPickUp();
        else
            itemPickUp.m_quantity = remainderQuantity;
    }
}
