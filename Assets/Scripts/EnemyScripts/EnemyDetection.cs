using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public bool m_playerInSight;

    //when player enters the enemy's detection circle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayer = collision.gameObject.layer;

        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            m_playerInSight = true;
        }
    }

    //when player exits the enemy's detection circle
    private void OnTriggerExit2D(Collider2D collision)
    {
        int collisionLayer = collision.gameObject.layer;

        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            m_playerInSight = false;
        }
    }
}
