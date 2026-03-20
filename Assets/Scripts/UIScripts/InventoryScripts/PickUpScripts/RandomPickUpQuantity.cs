using Unity.VisualScripting;
using UnityEngine;

public class RandomPickUpQuantity : MonoBehaviour
{
    [SerializeField] private ItemPickUp m_itemPickUp;
    [SerializeField] private EnemyType m_enemyType;

    private void Start()
    {
        switch (m_enemyType)
        {
            case EnemyType.Slime:
                m_itemPickUp.m_quantity = Random.Range(1, 3);
                break;
            case EnemyType.Wolf:
                m_itemPickUp.m_quantity = Random.Range(1, 2);
                break;
            case EnemyType.Bee:
                m_itemPickUp.m_quantity = 1;
                break;
            case EnemyType.Goblin:
                m_itemPickUp.m_quantity = Random.Range(3, 5);
                break;
            case EnemyType.MushBoss:
                m_itemPickUp.m_quantity = Random.Range(7, 9);
                break;
            default:
                m_itemPickUp.m_quantity = Random.Range(1, 2);
                break;
        }

    }
}
