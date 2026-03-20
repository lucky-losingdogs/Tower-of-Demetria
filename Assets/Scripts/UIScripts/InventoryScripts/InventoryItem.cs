using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    //reference to item image
    [SerializeField] private Image m_itemImage;

    //reference to the border around the item box
    [SerializeField] Image m_borderImage;

    //reference to the text showing the quantity of an item
    [SerializeField] private TMP_Text m_quantityTxt;

    public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void Deselect()
    {
        if (m_borderImage != null)
            m_borderImage.enabled = false;
    }

    public void ResetData()
    {
        if(m_itemImage != null && m_itemImage.gameObject != null)
        {
            m_itemImage.gameObject.SetActive(false);
        }
        empty = true;
    }

    //sets the item sprite and quantity text
    //in content side of inventory
    public void SetItemData(Sprite sprite, int quantity)
    {
        if (m_itemImage != null && m_itemImage.gameObject != null)
        {
            m_itemImage.gameObject.SetActive(true);
            m_itemImage.sprite = sprite;
        }

        if (m_quantityTxt != null)
        {
            //the quantity int into a string
            m_quantityTxt.text = quantity + "";
        }
           
        empty = false;
    }

    public void Select()
    {
        if (m_borderImage != null)
            m_borderImage.enabled = true;
    }


    public void OnBeginDrag()
    {
        //if what's being dragged is empty return
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this); 
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if(pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            Debug.Log("lmb click");
            OnItemClicked?.Invoke(this);
        }
    }

}
