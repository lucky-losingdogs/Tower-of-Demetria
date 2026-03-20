using UnityEngine;
using UnityEngine.EventSystems;

public class ShopDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //if the cursor is dragging the inv item that has this script attached
        //get the one it's dragging
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem != null)
        {
            //find the ui inventory page that the inventory item belongs to
            InventoryPage targetInventory = GetComponentInParent<InventoryPage>();
            if (targetInventory == null)
            {
                Debug.Log("Dropped item outside the InventoryPage");
                return;
            }

            //swaps the dragged item with the item that it's dropped on
            targetInventory.HandleDropOverSlot(draggedItem, gameObject.GetComponent<InventoryItem>());
            Debug.Log($"Dropped {draggedItem.name} on {gameObject.GetComponent<InventoryItem>().name} in {targetInventory.name}");
        }
    }
}
