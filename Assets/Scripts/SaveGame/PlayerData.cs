using System.Collections.Generic;
using UnityEngine;
using static InventorySO;

[System.Serializable]
public class PlayerData
{
    //the data that gets saved in a save file
    public float m_hp;
    public float[] m_position;
    public List<CropData> m_crops = new List<CropData>();
    public List<EnemyData> m_enemies = new List<EnemyData>();
    public List<InventoryItemData> m_inventoryItemsList = new List<InventoryItemData>();

    public PlayerData(GameObject player)
    {
        //player hp
        HP healthScript = player.GetComponent<HP>();
        m_hp = healthScript.m_currentHP;

        //player position
        m_position = new float[2];
        m_position[0] = player.transform.position.x;
        m_position[1] = player.transform.position.y;

        //current crop prefabs
        SaveCrops();
        //current enemy prefabs
        SaveEnemies();

        //current inventory
        SaveInventory(player);
    }

    //for each inventory item struct in the player's inventorySO, add item data to list
    private void SaveInventory(GameObject player)
    {
        InventoryController invController = player.GetComponent<InventoryController>();
        foreach (str_InventoryItem invItem in invController.inventoryData.m_inventoryItemsList)
        {
            if (invItem.m_isEmpty)
                continue;

            InventoryItemData data = new InventoryItemData
            {
                itemID = invItem.m_item.m_itemID,
                quantity = invItem.m_quantity
            };

            m_inventoryItemsList.Add(data);
        }
    }

    //for each crop prefab in the crop parent game object, add crop data to list
    private void SaveCrops()
    {
        Transform cropParent = GameObject.Find("CropParent").transform;

        foreach (Transform crop in cropParent)
        {
            CropController cropComponent = crop.GetComponent<CropController>();

            CropData data = new CropData
            {
                cropID = cropComponent.m_cropID,
                positionX = crop.position.x,
                positionY = crop.position.y
            };

            m_crops.Add(data);
        }
    }

    //for each enemy prefab in the enemy parent game object, add enemy data to list
    private void SaveEnemies()
    {
        Transform enemyParent = GameObject.Find("EnemyParent").transform;

        foreach (Transform enemy in enemyParent)
        {
            NavEnemyController enemyController = enemy.GetComponent<NavEnemyController>();

            EnemyData data = new EnemyData
            {
                enemyID = enemyController.enemyType,
                positionX = enemy.position.x,
                positionY = enemy.position.y,
                hp = enemy.GetComponent<HP>().m_currentHP
            };

            m_enemies.Add(data);
        }
    }

    [System.Serializable]
    public struct CropData
    {
        public CropID cropID;
        public float positionX;
        public float positionY; 
    }

    [System.Serializable]
    public struct EnemyData
    {
        public EnemyType enemyID;
        public float positionX;
        public float positionY;
        public float hp;
    }

    [System.Serializable]
    public struct InventoryItemData
    {
        public ItemID itemID;
        public int quantity;
    }

}

