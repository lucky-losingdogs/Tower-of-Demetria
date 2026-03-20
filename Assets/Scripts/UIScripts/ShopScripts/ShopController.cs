using System.Collections.Generic;
using UnityEngine;
using static InventorySO;

public class ShopController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;


    [SerializeField] private ShopPage ShopUI;
    [SerializeField] private InventorySO shopInventoryData;
    [SerializeField] private InventorySO playerInventoryData;
    public List<str_InventoryItem> m_shopInitialItemsList = new List<str_InventoryItem>();
    private List<str_InventoryItem> m_playerInitialItemsList = new List<str_InventoryItem>();

    private void Start()
    {
        InventoryPage playerShopUI = ShopUI.shopInventory01;
        InventoryPage shopUI = ShopUI.shopInventory02;

        if (playerShopUI == null|| shopUI == null)
        {
            Debug.Log("player or shop inventory page is missing");
        }

        List<str_InventoryItem> m_shopItemsList = PopulateShopInventory();

        inventoryController.PrepareTwoInvUI(playerShopUI, playerInventoryData, shopUI, shopInventoryData);

        inventoryController.PrepareTwoInvData(playerInventoryData, m_playerInitialItemsList, playerShopUI, shopInventoryData, m_shopItemsList, shopUI);
    }

    private List<str_InventoryItem> PopulateShopInventory()
    {
        List<str_InventoryItem> shopItemsList = new List<str_InventoryItem>();

        foreach (str_InventoryItem item in m_shopInitialItemsList)
        {
            shopItemsList.Add(item);
        }
        return shopItemsList;
    }

    public void ResetShop(InventoryPage shopUI)
    {
        inventoryController.PrepareInvData(shopInventoryData, m_shopInitialItemsList, shopUI);
    }

}
