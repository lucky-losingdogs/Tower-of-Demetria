using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class InventorySO : ScriptableObject
{
    [SerializeField] public List<str_InventoryItem> m_inventoryItemsList;

    [field: SerializeField] public int m_size { get; private set; } = 16;

    public event Action<Dictionary<int, str_InventoryItem>> OnInventoryUpdated;


    //initalise inventory list with empty items
    //inventory size based on m_size
    public void Initialise()
    {
        m_inventoryItemsList = new List<str_InventoryItem>();
        for (int i = 0; i < m_size; i++)
        {
            m_inventoryItemsList.Add(str_InventoryItem.GetEmptyItem());
        }
    }


    public int AddItem(ItemSO item, int quantity)
    {
        if (!item.m_isStackable)
        {
            while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToEmptySlot(item, 1);
                }

                InformUIOfChange();
                return quantity;
        }

        quantity = AddStackableItem(item, quantity);
        InformUIOfChange();
        return quantity;
    }


    public void AddItem(str_InventoryItem item)
    {
        AddItem(item.m_item, item.m_quantity);
    }

    public void SetItem(int index, str_InventoryItem item)
    {
        if (index < 0 || index >= m_inventoryItemsList.Count)
            return;

        m_inventoryItemsList[index] = item;
        InformUIOfChange();
    }


    //indexes through list, returns false if there is an empty slot
    private bool IsInventoryFull()
    {
        for (int i = 0; i < m_inventoryItemsList.Count; i++)
        {
            if (m_inventoryItemsList[i].m_isEmpty)
                return false;
        }

        return true;
    }

    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < m_inventoryItemsList.Count; i++)
        {
            //checks if there is a filled inv slot
            //continue if it's empty
            if (m_inventoryItemsList[i].m_isEmpty)
                continue;

            //checks if the item's id in the list matches the new item's id
            if (m_inventoryItemsList[i].m_item.ID == item.ID)
            {
                //the the remaining amount an item stack can carry
                //before needing to create another slot for the rest of the items
                int amountCanTake = m_inventoryItemsList[i].m_item.m_maxStackSize - m_inventoryItemsList[i].m_quantity;

                //if the quantity is greater than the stack space avaialable
                if (quantity > amountCanTake)
                {
                    //change the quantity of the item on the list to the max stack size
                    m_inventoryItemsList[i] = m_inventoryItemsList[i].ChangeQuantity(m_inventoryItemsList[i].m_item.m_maxStackSize);

                    //then subtract the quantity of the item by how much space was available
                    //for the remaining quantity
                    quantity -= amountCanTake;
                }
                else
                {
                    //if the quantity is smaller than the stack space available
                    //add the quantity of the item to the quantity of the item on the list
                    m_inventoryItemsList[i] = m_inventoryItemsList[i].ChangeQuantity(m_inventoryItemsList[i].m_quantity + quantity);
                    InformUIOfChange();
                    return 0;
                }
            }
        }

        //if the quantity of item was larger than the max stack size
        while (quantity > 0 && !IsInventoryFull())
        {
            //clamp the quantity between the range of 0 and max stack size
            int newQuantity = Mathf.Clamp(quantity, 0, item.m_maxStackSize);

            //add the remaining amount to an empty slot
            AddItemToEmptySlot(item, newQuantity);

            //subtract quantity from the new quantity that is added to a new slot to end the while loop
            quantity -= newQuantity;
        }
        //if the inventory is full, stores the remainder that couldn't fit in the inventory
        return quantity;
    }

    //adds nonstackable item to the inventory to an index in the list that is empty
    private int AddItemToEmptySlot(ItemSO item, int quantity)
    {
        str_InventoryItem newItem = new str_InventoryItem
        {
            m_item = item,
            m_quantity = quantity
        };

        for (int i = 0; i < m_inventoryItemsList.Count; i++)
        {
            if (m_inventoryItemsList[i].m_isEmpty)
            {
                m_inventoryItemsList[i] = newItem;
                return quantity;
            }
        }

        return 0;
    }

    //updating only specific indexed items
    //so all of them don't get updated if they don't need to
    public Dictionary<int, str_InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, str_InventoryItem> returnValue = new Dictionary<int, str_InventoryItem>();

        for (int i = 0; i < m_inventoryItemsList.Count; i++)
        {
            if (m_inventoryItemsList[i].m_isEmpty)
                continue;

            returnValue[i] = m_inventoryItemsList[i];
        }
        return returnValue;
    }

    public str_InventoryItem GetItemFromList(int itemIndex)
    {
        return m_inventoryItemsList[itemIndex];
    }

    //swaps index of items
    public void SwapItems(int itemIndex01, int itemIndex02)
    {
        str_InventoryItem tempItem = m_inventoryItemsList[itemIndex01];
        m_inventoryItemsList[itemIndex01] = m_inventoryItemsList[itemIndex02];
        m_inventoryItemsList[itemIndex02] = tempItem;

        InformUIOfChange();
    }

    private void InformUIOfChange()
    {
        //calls the function that generates the dictionary
        //with the current inv state
        OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    }

    public void RemoveItem(int itemIndex, int quantity)
    {
        //if the item index is smaller than the inventory items list
        //the index of the item should be within the list
        if (m_inventoryItemsList.Count > itemIndex)
        {
            //if that index is already empty return
            if (m_inventoryItemsList[itemIndex].m_isEmpty)
                return;

            //remainder is the amount left over after the quantity being removed
            //is subtracted from the quantity of the item in the list
            int remainder = m_inventoryItemsList[itemIndex].m_quantity - quantity;

            //if the remainder is 0 or less, set that index as an empty item
            if (remainder <= 0)
                m_inventoryItemsList[itemIndex] = str_InventoryItem.GetEmptyItem();
            //else, change the quantity of the item in the list to the remainder
            else
                m_inventoryItemsList[itemIndex] = m_inventoryItemsList[itemIndex].ChangeQuantity(remainder);

            InformUIOfChange();

        }
    }

    public void Clear()
    {
        // Replace all slots with empty items
        for (int i = 0; i < m_inventoryItemsList.Count; i++)
        {
            m_inventoryItemsList[i] = str_InventoryItem.GetEmptyItem();
        }

        // Inform the UI that the inventory has been cleared
        InformUIOfChange();
    }



    [Serializable]
    public struct str_InventoryItem
    {
        public int m_quantity;
        public ItemSO m_item;

        //returns true when m_item is null.
        public bool m_isEmpty => m_item == null;

        //takes the same item but modifies the quantity
        //for multiple stacks of the same item
        public str_InventoryItem ChangeQuantity(int newQuantity)
        {
            return new str_InventoryItem
            {
                m_item = this.m_item,
                m_quantity = newQuantity
            };
        }

        //cant usually set struct members with null values
        public static str_InventoryItem GetEmptyItem()
        {
            return new str_InventoryItem
            {
                m_item = null,
                m_quantity = 0
            };
        }

    }
}