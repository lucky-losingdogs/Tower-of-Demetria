using System;
using System.Collections.Generic;
using UnityEngine;
using static InventorySO;

public class TradeDatabase : MonoBehaviour
{
    [SerializeField] private List<ShopTrade> shopTrades;

    private Dictionary<ItemID, ShopTrade> tradeLookup;

    //put the serialized list into the dictionary 
    private void Awake()
    {
        tradeLookup = new Dictionary<ItemID, ShopTrade>();

        foreach (var trade in shopTrades)
        {
            tradeLookup[trade.itemToBuy] = trade;
        }
    }

    //returns true if the player has enough of an item to trade for an item in the shop
    public bool CanAfford(ItemID itemID, InventorySO playerInventory)
    {
        if (!tradeLookup.ContainsKey(itemID))
            return false;

        ShopTrade itemToBuy = tradeLookup[itemID];

        foreach (var cost in itemToBuy.cost)
        {
            //if the item cost is free, return true
            //so the player can take it without needing anything to exchange
            if (cost.quantity == 0)
                return true;
            
            int totalOwned = GetTotalItemCount(playerInventory, cost.itemID);

            if (totalOwned < cost.quantity)
                return false;
        }
        return true;
    }

    //check the player's inventory for the quantity of a specific item
    private int GetTotalItemCount(InventorySO playerInventory, ItemID itemID)
    {
        int itemsInInv = 0;

        for (int i = 0; i < playerInventory.m_inventoryItemsList.Count; i++)
        {
            str_InventoryItem item = playerInventory.m_inventoryItemsList[i];

            if (!item.m_isEmpty && item.m_item.m_itemID == itemID)
            {
                itemsInInv += item.m_quantity;
            }
        }

        return itemsInInv;
    }

    //total together the cost of the item to remove from the player's inventory
    public void TradeItem(ItemID itemID, InventorySO playerInventory)
    {
        ShopTrade itemToBuy = tradeLookup[itemID];

        foreach (var cost in itemToBuy.cost)
        {
            //if the item cost is free, nothing is removed from player inventory
            if (cost.quantity == 0)
                return;

            RemoveItemsFromInventory(playerInventory, cost.itemID, cost.quantity);
        }
    }

    //remove the traded items from the player's inventory
    private void RemoveItemsFromInventory(InventorySO playerInventory, ItemID itemID, int quantity)
    {
        //iterate through the inventory
        for (int i = 0; i < playerInventory.m_inventoryItemsList.Count; i++)
        {
            str_InventoryItem item = playerInventory.m_inventoryItemsList[i];

            if (quantity <= 0)
                break;

            //remove the indexed item matching the correct item id
            if (!item.m_isEmpty && item.m_item.m_itemID == itemID)
            {
                int removeAmount = Mathf.Min(item.m_quantity, quantity);
                playerInventory.RemoveItem(i, removeAmount);
                quantity -= removeAmount;
            }
        }
    }
}


//the item and quantity pair
[Serializable]
public class ItemCost
{
    public ItemID itemID;
    public int quantity;
}

//the costs of items in exchange for a quantity of another item 
[Serializable]
public class ShopTrade
{
    public ItemID itemToBuy;
    public List<ItemCost> cost;
}

