using UnityEngine;
using static PlayerData;

public class AssignSaveData : MonoBehaviour
{
    public GameObject m_player;
    [SerializeField] private Transform m_cropParent;
    [SerializeField] private Transform m_enemyParent;

    [SerializeField] private SpawnCropPrefab m_spawnCropPrefab;
    [SerializeField] private EnemyWaves m_enemyWaves;
    [SerializeField] private ItemDataBase m_itemDatabase;

    public void AssignLoadedData(PlayerData data)
    {
        //player hp reloaded
        HP hp = m_player.GetComponent<HP>();
        hp.m_currentHP = data.m_hp;
        hp.UpdatePlayerUI();

        //player position reloaded
        Vector2 savedPosition = new Vector2(data.m_position[0], data.m_position[1]);
        m_player.transform.position = savedPosition;


        //player inventory reloaded
        InventoryController invController = m_player.GetComponent<InventoryController>();
        InventorySO inventory = invController.inventoryData;

        //clear current inventory
        inventory.Initialise();

        foreach (var itemData in data.m_inventoryItemsList)
        {
            ItemSO item = m_itemDatabase.MatchItemID(itemData.itemID);
            inventory.AddItem(item, itemData.quantity);
        }


        //clears the current world
        ClearChildren(m_cropParent);
        ClearChildren(m_enemyParent);


        //load crops
        foreach (CropData cropData in data.m_crops)
        {
            GameObject prefab = m_spawnCropPrefab.MatchCropID(cropData.cropID);

            if (prefab != null)
            {
                GameObject crop = Instantiate(prefab, m_cropParent);
                crop.transform.position = new Vector2(cropData.positionX, cropData.positionY);
            }
        }


        //load enemies
        foreach (var enemyData in data.m_enemies)
        {
            GameObject prefab = m_enemyWaves.MatchEnemyID(enemyData.enemyID);

            if (prefab != null)
            {
                GameObject enemy = Instantiate(prefab, m_enemyParent);
                enemy.transform.position = new Vector2(enemyData.positionX, enemyData.positionY);

                enemy.GetComponent<HP>().m_currentHP = enemyData.hp;
            }
        }
    }


    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
