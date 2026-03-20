using UnityEngine;
using System.Collections;
using System;

public class ItemPickUp : MonoBehaviour
{
    [field: SerializeField] public ItemSO m_item { get; private set; }

    [field: SerializeField] public int m_quantity { get; set; } = 1;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float duration = 0.3f;

    [SerializeField] private InventorySO m_playerInventoryData;

    private void Start()
    {
        //sets the sprite of the item pick up to the image of the ItemSO
        GetComponent<SpriteRenderer>().sprite = m_item.m_itemImage;
        StartCoroutine(AutoPickTimer());
    }

    //called when an item is picked up so you only pick it up once
    public void DestroyPickUp()
    {
        //check to see if 2d collider is not null
        Collider2D m_collider2d = GetComponent<Collider2D>();
        if (m_collider2d != null )
        {
            //disables 2d collider
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(AnimatePickUp());
        }
    }

    private IEnumerator AnimatePickUp()
    {
        //plays sound effect
        audioSource.Play();

        //shrinks the item until invisible when its picked up
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator AutoPickTimer()
    {
        yield return new WaitForSeconds(10);

        if (gameObject != null )
        {
            AddPickUp(this);
            Destroy(gameObject);
        }
    }

    public void AddPickUp(ItemPickUp itemPickUp)
    {
        //add the item to InventorySO
        //if inventory is full, the remainder is returned here
        int remainderQuantity = m_playerInventoryData.AddItem(itemPickUp.m_item, itemPickUp.m_quantity);

        if (remainderQuantity == 0)
            itemPickUp.DestroyPickUp();
        else
            itemPickUp.m_quantity = remainderQuantity;
    }
}
