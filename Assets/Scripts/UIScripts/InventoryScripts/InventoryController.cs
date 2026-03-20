using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InventorySO;

public class InventoryController : MonoBehaviour
{
    #region references and variables

    public InventoryPage inventoryUI;

    public InventorySO inventoryData;

    private Dictionary<InventoryPage, InventorySO> inventoryMap = new Dictionary<InventoryPage, InventorySO>();


    [SerializeField] private AudioClip dropClip;

    [SerializeField] private AudioSource audioSource;

    private InputAction m_inventoryAction;

    public List<str_InventoryItem> m_initialItemsList = new List<str_InventoryItem>();

    [SerializeField] private TradeDatabase m_tradeDatabase;
    [SerializeField] private InventoryPage shopInventoryUI;

    #endregion


    //binds the i key to inventoryAction
    private void Awake()
    {
        m_inventoryAction = InputSystem.actions.FindAction("Inventory");
    }
    private void Start()
    {
        PrepareInvUI(inventoryUI, inventoryData);
        PrepareInvData(inventoryData, m_initialItemsList, inventoryUI);
    }

    //checks if the i key has been pressed and
    //shows or hides the inventory
    public void Update()
    {
        if (m_inventoryAction.WasPressedThisFrame())
        {
            if (!inventoryUI.isActiveAndEnabled)
            {
                MenuSorting.CloseAllMenus();
                inventoryUI.Show();

                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.m_item.m_itemImage, item.Value.m_quantity);
                }
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }

    //initialises inventory with the inventory size given by InventorySO in the inspector
    public void PrepareInvUI(InventoryPage inventoryUI, InventorySO inventoryData)
    {
        inventoryMap[inventoryUI] = inventoryData;
        inventoryUI.InitialiseInventoryUI(inventoryData.m_size);

        inventoryUI.OnDescriptionRequested += (inventoryUI, index) => HandleDescriptionRequest(index, inventoryUI, inventoryData);
        inventoryUI.OnItemActionRequested += (inventoryUI, index) => HandleItemActionRequest(index, inventoryUI, inventoryData);
        inventoryUI.OnStartDragging += (inventoryUI, index) => HandleStartDragging(index, inventoryUI, inventoryData);
        inventoryUI.OnSwapItems += HandleSwapItems;
    }

    public void PrepareTwoInvUI(InventoryPage playerInvUI, InventorySO playerInvData, InventoryPage shopInvUI, InventorySO shopInvData)
    {
        //player inventory
        inventoryMap[playerInvUI] = playerInvData;
        playerInvUI.InitialiseInventoryUI(playerInvData.m_size);

        playerInvUI.OnDescriptionRequested += (playerInvUI, index) => HandleDescriptionRequest(index, playerInvUI, playerInvData);
        playerInvUI.OnItemActionRequested += (playerInvUI, index) => HandleItemActionRequest(index, playerInvUI, playerInvData);
        playerInvUI.OnStartDragging += (playerInvUI, index) => HandleStartDragging(index, playerInvUI, playerInvData);
        playerInvUI.OnSwapItems += HandleSwapItems;


        //shop inventory
        inventoryMap[shopInvUI] = shopInvData;
        shopInvUI.InitialiseInventoryUI(shopInvData.m_size);

        shopInvUI.OnDescriptionRequested += (shopInvUI, index) => HandleDescriptionRequest(index, shopInvUI, shopInvData);
        shopInvUI.OnItemActionRequested += (shopInvUI, index) => HandleItemActionRequest(index, shopInvUI, shopInvData);
        shopInvUI.OnStartDragging += (shopInvUI, index) => HandleStartDragging(index, shopInvUI, shopInvData);
        shopInvUI.OnSwapItems += HandleSwapItems;
    }

    public void PrepareInvData(InventorySO inventoryData, List<str_InventoryItem> m_initialItemsList, InventoryPage inventoryUI)
    {
        inventoryData.Initialise();

        inventoryData.OnInventoryUpdated += (OnInventoryUpdated) => HandleUpdateInventoryUI(OnInventoryUpdated, inventoryUI);

        foreach (var item in m_initialItemsList)
        {
            if (item.m_isEmpty)
                continue;

            inventoryData.AddItem(item);
        }
    }

    public void PrepareTwoInvData(InventorySO playerInvData, List<str_InventoryItem> m_playerInitialItemsList, InventoryPage playerInvUI, InventorySO shopInvData, List<str_InventoryItem> m_shopinitialItemsList, InventoryPage shopInvUI)
    {
        //player inventory
        playerInvData.Initialise();

        playerInvData.OnInventoryUpdated += (OnInventoryUpdated) => HandleUpdateInventoryUI(OnInventoryUpdated, playerInvUI);

        foreach (var item in m_playerInitialItemsList)
        {
            if (item.m_isEmpty)
                continue;

            playerInvData.AddItem(item);
        }


        //shop inventory
        shopInvData.Initialise();

        shopInvData.OnInventoryUpdated += (OnInventoryUpdated) => HandleUpdateInventoryUI(OnInventoryUpdated, shopInvUI);

        foreach (var item in m_shopinitialItemsList)
        {
            if (item.m_isEmpty)
                continue;

            shopInvData.AddItem(item);
        }
    }

    private void HandleUpdateInventoryUI(Dictionary<int, str_InventoryItem> inventoryState, InventoryPage inventoryUI)
    {
        inventoryUI.ResetAllItems();

        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.m_item.m_itemImage, item.Value.m_quantity);
        }
    }

    private void HandleSwapItems(InventoryPage fromPage, int fromIndex, InventoryPage toPage, int toIndex)
    {
        InventorySO fromData = inventoryMap[fromPage];
        InventorySO toData = inventoryMap[toPage];

        //if the inventories being swapped are the same one, just swap inside the one
        if (fromData == toData)
        {
            str_InventoryItem item = fromData.GetItemFromList(fromIndex);
            fromData.SwapItems(fromIndex, toIndex);
            return;
        }

        //if the inventories are different (like in the shop) transfer between inventories
        str_InventoryItem fromItem = fromData.GetItemFromList(fromIndex);
        str_InventoryItem toItem = toData.GetItemFromList(toIndex);

        if (fromItem.m_isEmpty)
            return;

        //if it's being taken from the store
        if (fromPage == shopInventoryUI)
        {
            if (!TradeWithStore(fromItem, toData))
                return;
        }

        AddStack(toItem, fromItem, toData, toIndex, fromData, fromIndex);


        //swap directly between inventories
        fromData.SetItem(fromIndex, toItem);
        toData.SetItem(toIndex, fromItem);
    }

    private void HandleStartDragging(int itemIndex, InventoryPage inventoryUI, InventorySO inventoryData)
    {
        str_InventoryItem inventoryItem = inventoryData.GetItemFromList(itemIndex);

        if (inventoryItem.m_isEmpty)
            return;

        inventoryUI.CreateDraggedItem(inventoryItem.m_item.m_itemImage, inventoryItem.m_quantity);
    }

    private void HandleItemActionRequest(int itemIndex, InventoryPage inventoryUI, InventorySO inventoryData)
    {
        str_InventoryItem inventoryItem = inventoryData.GetItemFromList(itemIndex);
        if (inventoryItem.m_isEmpty)
            return;

        IItemAction itemAction = inventoryItem.m_item as IItemAction;
        if (itemAction != null)
        {
            //toggles the action panel over the item
            inventoryUI.ShowItemActionPanel(itemIndex);

            //changes the button to show the correct action name and performs the action on click
            inventoryUI.AddAction(itemAction.m_actionName, () => PerformAction(itemIndex, inventoryData, inventoryUI));
        }

        IDestroyableItem destroyableItem = inventoryItem.m_item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.m_quantity, inventoryUI, inventoryData));
        }
    }

    private void HandleDescriptionRequest(int itemIndex, InventoryPage inventoryUI, InventorySO inventoryData)
    {
        //gets item data from InventorySO list based on index
        str_InventoryItem inventoryItem = inventoryData.GetItemFromList(itemIndex);

        //if item is empty return
        if (inventoryItem.m_isEmpty)
        {
            inventoryUI.ResetSelectDesc();
            return;
        }

        //creates ItemSO instance with inventoryItem data
        ItemSO item = inventoryItem.m_item;
        inventoryUI.UpdateDescription(itemIndex, item.m_itemImage, item.m_itemName, item.m_itemDescription);
    }

    private void PerformAction(int itemIndex, InventorySO inventoryData, InventoryPage inventoryUI)
    {
        str_InventoryItem inventoryItem = inventoryData.GetItemFromList(itemIndex);
        if (inventoryItem.m_isEmpty)
            return;

        IDestroyableItem destroyableItem = inventoryItem.m_item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryData.RemoveItem(itemIndex, 1);
        }

        IItemAction itemAction = inventoryItem.m_item as IItemAction;
        if (itemAction != null)
        {
            int m_quantity = inventoryItem.m_quantity;
            Debug.Log(m_quantity);

            //remove item from inventory
            inventoryData.RemoveItem(itemIndex, 1);

            //perform the action
            itemAction.PerformAction(gameObject, m_quantity);

            //play audioclip
            audioSource.PlayOneShot(itemAction.m_actionSFX);

            //reset description and selection and action panel after use
            inventoryUI.ResetSelectDesc();
        }
    }

    //action to remove item from inventory
    private void DropItem(int itemIndex, int m_quantity, InventoryPage inventoryUI, InventorySO inventoryData)
    {
        inventoryData.RemoveItem(itemIndex, m_quantity);
        inventoryUI.ResetSelectDesc();
        audioSource.PlayOneShot(dropClip);
    }

    private bool TradeWithStore(str_InventoryItem fromItem, InventorySO toData)
    {
        ItemID buyingID = fromItem.m_item.m_itemID;

        //check if the player can afford the item
        if (!m_tradeDatabase.CanAfford(buyingID, toData))
        {
            Debug.Log("Can't afford");
            return false;
        }
        //if they can afford it, allow the item swap
        m_tradeDatabase.TradeItem(buyingID, toData);
        return true;
    }

    private void AddStack(str_InventoryItem toItem, str_InventoryItem fromItem, InventorySO toData, int toIndex, InventorySO fromData, int fromIndex)
    {
        //if the item being dropped on isn't empty,
        //and if the dragged item matches the dropped on item
        //and if the items are stackable
        if (!toItem.m_isEmpty && fromItem.m_item.ID == toItem.m_item.ID && fromItem.m_item.m_isStackable)
        {
            int totalQuantity = fromItem.m_quantity + toItem.m_quantity;
            int maxStack = fromItem.m_item.m_maxStackSize;

            if (totalQuantity <= maxStack)
            {
                //fit it all in one stack
                toData.SetItem(toIndex, fromItem.ChangeQuantity(totalQuantity));
                fromData.SetItem(fromIndex, str_InventoryItem.GetEmptyItem());
            }
            else
            {
                //fill the target stack to max, leave remainder in source
                toData.SetItem(toIndex, fromItem.ChangeQuantity(maxStack));
                fromData.SetItem(fromIndex, fromItem.ChangeQuantity(totalQuantity - maxStack));
            }

            return;
        }
    }
}
