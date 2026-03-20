using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MouseFollower : MonoBehaviour
{
    //reference to canvas
    [SerializeField] private Canvas m_canvas;

    //reference to main camera
    [SerializeField] private Camera m_camera;

    //reference to InventoryItem script
    [SerializeField] private InventoryItem m_item;

    public void Awake()
    {
        //reference to parent bc canvas not in the mouse follower object itself
        m_canvas = GetComponentInParent<Canvas>();

        m_camera = Camera.main;

        //reference to child bc inventory item not in the mouse follower object itself
        m_item = GetComponentInChildren<InventoryItem>();
    }

    public void SetItemData(Sprite sprite, int quantity)
    {
        m_item.SetItemData(sprite, quantity);
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void Toggle(bool value)
    {
        gameObject.SetActive(value);
    }
}
