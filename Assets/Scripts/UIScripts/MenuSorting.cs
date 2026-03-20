using System;
using UnityEngine;

public class MenuSorting : MonoBehaviour
{
    private static MenuSorting s_instance;
    
    [SerializeField] InventoryPage m_inventoryUI;
    [SerializeField] GameObject m_pauseUI;
    [SerializeField] OpenShop m_openShop;

    public event Action OnMenuOpen;

    private void Start()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        
        OnMenuOpen += StopMenuOverlap;
    }

    private void StopMenuOverlap()
    {
        m_inventoryUI.Hide();
        m_pauseUI.SetActive(false);
        m_openShop.m_shopUIBg.SetActive(false);
        m_openShop.playerInventoryUI01.Hide();
        m_openShop.shopInventoryUI02.Hide();
    }

    public static void CloseAllMenus()
    {
        MenuSorting.s_instance.OnMenuOpen?.Invoke();
    }

}
