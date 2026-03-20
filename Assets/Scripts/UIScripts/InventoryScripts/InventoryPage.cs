using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InventorySO;

public class InventoryPage : MonoBehaviour
{
    #region References
    [Header ("Inventory References")]
    //reference to the itemUI prefab
    [SerializeField] private InventoryItem m_itemPrefab;

    //reference to the panel where all the itemUI prefabs will be cloned
    [SerializeField] private RectTransform m_contentPanel;

    //reference to the item description section
    [SerializeField] private InventoryDescription m_itemDescription;

    //reference to the mouse follower game object
    [SerializeField] private MouseFollower m_mouseFollower;

    //reference to the action panel
    [SerializeField] private ItemActionPanel m_actionPanel;

    [SerializeField] private GameObject m_inventoryBorder;

    #endregion


    #region Events

    public event Action<InventoryPage, int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;

    public event Action<InventoryPage, int, InventoryPage, int> OnSwapItems;

    #endregion


    List<InventoryItem> uiItemList = new List<InventoryItem>();

    //outside bounds of uiItemList
    private int m_currentlyDraggedItemIndex = -1;

    //the inventory that the item is being dragged from
    public static InventoryPage draggedInventory;


    private void Awake()
    {
        //automatically hides the inventory
        Hide();

        //turns off the mouse follower on start
        m_mouseFollower.Toggle(false);

        //resets item desc on start
        m_itemDescription.ResetDescription();
    }

    //adds an item to the uiItemList equivalent to the given inventorySize
    //and instantiates that number of items
    public void InitialiseInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem uiItem = Instantiate(m_itemPrefab, m_contentPanel, false);
            //uiItem.transform.SetParent(m_contentPanel);
            uiItemList.Add(uiItem);

            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            //uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (uiItemList.Count > itemIndex)
        {
            uiItemList[itemIndex].SetItemData(itemImage, itemQuantity);
        }
    }

    public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string itemDescription)
    {
        m_itemDescription.SetDescription(itemImage, itemName, itemDescription);

        DeselectAllItems();
        uiItemList[itemIndex].Select();
    }

    private void HandleShowItemActions(InventoryItem item)
    {
        int index = uiItemList.IndexOf(item);
        if (index == -1)
            return;
        
        OnItemActionRequested?.Invoke(this, index);   
    }

    private void HandleEndDrag(InventoryItem item)
    {
        ResetDraggedItem();
    }

    //private void HandleSwap(InventoryItem item)
    //{
    //    int index = uiItemList.IndexOf(item);

    //    //if the item being swapped with has no index it's not
    //    //on the item list and is not swappable
    //    if (m_currentlyDraggedItemIndex == -1)
    //        return;

    //    OnSwapItems?.Invoke(DraggedInventory, m_currentlyDraggedItemIndex, this, index);
    //}

    private void ResetDraggedItem()
    {
        //toggles off the mouse follower dragging
        m_mouseFollower.Toggle(false);

        //resets to -1
        m_currentlyDraggedItemIndex = -1;
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        int index = uiItemList.IndexOf(item);
        
        //if the item being dragged has no index it's not
        //on the item list and is not draggable
        if (index == -1)
            return;

        m_currentlyDraggedItemIndex = index;

        draggedInventory = this;

        HandleItemSelection(item);

        OnStartDragging?.Invoke(this, index);
    }

    public void CreateDraggedItem(Sprite itemImage, int quantity)
    {
        //toggle on mouse follower
        m_mouseFollower.Toggle(true);

        m_mouseFollower.SetItemData(itemImage, quantity);
    }
    
    private void HandleItemSelection(InventoryItem item)
    {
        int index = uiItemList.IndexOf(item);

        //if the index is outside of the list return
        if (index == -1)
            return;

        //invokes OnDescriptionRequested event -> HandleDescriptionRequest in InventoryController
        OnDescriptionRequested?.Invoke(this, index);
    }

    //shows the inventory ui
    public void Show()
    {
        //pauses time when the inventory menu is open
        Time.timeScale = 0f;

        //reveals the inventory menu ui
        gameObject.SetActive(true);
        m_inventoryBorder.SetActive(true);

        //ensure that item description and selection
        //is reset when the inventory is opened
        ResetSelectDesc();
    }

    //hides the inventory ui
    public void Hide()
    {
        //unpauses time when the inventory menu is closed
        Time.timeScale = 1f;

        //hides the inventory menu ui
        gameObject.SetActive(false);
        m_inventoryBorder.SetActive(false);

        ResetDraggedItem();

        //hides the action panel ui
        m_actionPanel.Toggle(false);
    }

    //turns off selection border and resets selection
    private void DeselectAllItems()
    {
        foreach (InventoryItem item in uiItemList)
        {
            item.Deselect();
        }
    }

    //resets selection and resets description
    public void ResetSelectDesc()
    {
        m_itemDescription.ResetDescription();
        DeselectAllItems();
        m_actionPanel.Toggle(false);
    }

    public void ResetAllItems()
    {
        foreach (var item in uiItemList)
        {
            item.ResetData();
            item.Deselect();
        }
    }

    public void ShowItemActionPanel(int itemIndex)
    {
        m_actionPanel.Toggle(true);
        m_actionPanel.transform.position = uiItemList[itemIndex].transform.position;
    }

    public void AddAction(string actionName, Action performAction)
    {
        m_actionPanel.AddButton(actionName, performAction);
    }

    public void HandleDropOverSlot(InventoryItem draggedItem, InventoryItem targetSlot)
    {
        if (draggedInventory == null || draggedItem == null || targetSlot == null)
            return;

        //gets the dragged index from the dragged inventory
        int fromIndex = draggedInventory.m_currentlyDraggedItemIndex;

        //gets the target index from the target inventory
        int toIndex = uiItemList.IndexOf(targetSlot);

        if (fromIndex == -1 || toIndex == -1)
            return;

        //invoke swap event in InventoryController
        OnSwapItems?.Invoke(draggedInventory, fromIndex, this, toIndex);

        //reset dragging
        draggedInventory.ResetDraggedItem();
        draggedInventory = null;
    }
}
