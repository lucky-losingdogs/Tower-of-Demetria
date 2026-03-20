using UnityEngine;
using UnityEngine.UI;

public class OpenShop : MonoBehaviour
{
    [SerializeField] private ShopPage shopPage;
    
    public InventoryPage playerInventoryUI01;
    public InventoryPage shopInventoryUI02;
    [SerializeField] private InventorySO playerInventoryData;
    [SerializeField] private InventorySO shopInventoryData;
    [SerializeField] public GameObject m_shopUIBg;
    [SerializeField] private ShopController m_shopController;
    [SerializeField] private AudioClip m_shopAudioClip;

    private void Start()
    {
        playerInventoryUI01 = shopPage.shopInventory01;
        shopInventoryUI02 = shopPage.shopInventory02;

        playerInventoryUI01.Hide();
        shopInventoryUI02.Hide();

        m_shopUIBg.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (!playerInventoryUI01.isActiveAndEnabled && !shopInventoryUI02.isActiveAndEnabled)
        {
            SFXManager.PlayAudio(m_shopAudioClip);

            playerInventoryUI01.Show();
            shopInventoryUI02.Show();
            m_shopUIBg.SetActive(true);

            m_shopController.ResetShop(shopInventoryUI02);

            UpdateShopPages(playerInventoryData, playerInventoryUI01);
            UpdateShopPages(shopInventoryData, shopInventoryUI02);
        }
    }

    private void UpdateShopPages(InventorySO inventoryData, InventoryPage inventoryUI)
    {
        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateData(item.Key, item.Value.m_item.m_itemImage, item.Value.m_quantity);
        }
    }

    public void CloseShopButton()
    {
        playerInventoryUI01.Hide();
        shopInventoryUI02.Hide();
        m_shopUIBg.SetActive(false);
    }
}
