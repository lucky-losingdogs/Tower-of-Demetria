using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool m_gameRunning = true;

    private GameObject m_player;
    private HP m_playerHP;
    ItemPickUp[] pickups;

    [SerializeField] private InventorySO inventoryData;

    //on start, reset everything that could persist between scenes
    private void Awake()
    {
        if (m_player == null)
            m_player = GameObject.Find("Character");
        m_playerHP = m_player.GetComponent<HP>();
        pickups = FindObjectsOfType<ItemPickUp>();


        RestartGameState();
    }

    public void RestartGameState()
    {
        m_gameRunning = true;
        EnemyWaves.m_enemyCount = 0;
        m_playerHP.ResetHP();

        //clear the inventory
        if (inventoryData != null)
        {
            inventoryData.Clear();
        }

        //destroy all item picks up left in scene
        foreach (var pickup in pickups)
        {
            Destroy(pickup.gameObject);
        }
    }
}
