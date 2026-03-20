using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescription : MonoBehaviour
{
    [SerializeField] private Image m_itemImage;

    [SerializeField] private TMP_Text m_title;

    [SerializeField] private TMP_Text m_description;

    //ensures desc is empty at the start
    public void Awake()
    {
        ResetDescription();
    }

    //resetting title and desc to empty strings
    //and the item image to inactive
    public void ResetDescription()
    {
        m_itemImage.gameObject.SetActive(false);
        m_title.text = "";
        m_description.text = "";
    }

    //setting the item image, title and desc to
    //what is entered in the inspector
    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        m_itemImage.gameObject.SetActive(true);
        m_itemImage.sprite = sprite;
        m_title.text = itemName;
        m_description.text = itemDescription;
    }
}
