using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public CropID m_cropID;

    private CropStates m_cropStates = CropStates.Idle;

    private bool m_enemyInSight = false;
    private Transform m_enemyPos;
    private List<GameObject> m_detectedEnemies = new List<GameObject>();

    #region Crop Behaviour Scripts

    [SerializeField] private CropFireProjectile m_turnipBehaviour;
    [SerializeField] private TomatoExplode m_tomatoBehaviour;
    [SerializeField] private MelonFireFreezing m_melonBehaviour;
    [SerializeField] private CropFireMultipleProjectiles m_grapeBehaviour;
    [SerializeField] private CropMeleeAttack m_strawberryBehaviour;

    #endregion

    private void Update()
    {
        //state transitions based on detection
        if (m_enemyInSight)
        {
            m_cropStates = CropStates.Attack;
        }
        else
        {
            m_cropStates = CropStates.Idle;
        }

        switch (m_cropStates)
        {
            case CropStates.Idle:
                break;
            case CropStates.Attack:
                
                switch (m_cropID)
                {
                    case CropID.Turnip:
                        if (m_enemyPos != null)
                        {
                            if (m_turnipBehaviour == null)
                            {
                                Debug.Log("turnip behaviour is missing");
                                return;
                            }

                            m_turnipBehaviour.FireProjectile(m_enemyPos);
                        }
                        break;

                    case CropID.Tomato:
                        if (m_tomatoBehaviour == null)
                        {
                            Debug.Log("tomato behaviour is missing");
                            return;
                        }
                        m_tomatoBehaviour.TomatoAttack(m_detectedEnemies);
                        break;

                    case CropID.Melon:
                        if (m_melonBehaviour == null)
                        {
                            Debug.Log("tomato behaviour is missing");
                            return;
                        }
                        m_melonBehaviour.MoveFreezingAura(m_enemyPos);
                        break;
                    case CropID.Grape:
                        if (m_grapeBehaviour == null)
                        {
                            Debug.Log("grape behaviour is missing");
                            return;
                        }
                        m_grapeBehaviour.FireProjectiles(m_enemyPos);
                        break;

                    case CropID.Strawberry:
                        if (m_strawberryBehaviour == null)
                        {
                            Debug.Log("strawb behaviour is missing");
                            return;
                        }
                        m_strawberryBehaviour.MeleeAttack(m_detectedEnemies);
                        break;
                }
                break;
        }
    }


    //when enemy enters the crop's detection circle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //so that it doesn't collide with the enemy's detection circle
        if (collision.isTrigger)
            return;

        int collisionLayer = collision.gameObject.layer;

        //if the collider has the EnemyBody layer - ignores the enemy's detection circle
        if (collisionLayer == LayerMask.NameToLayer("EnemyBody"))
        {
            //if the enemy isn't on the detected enemies list, it adds it
            if (!m_detectedEnemies.Contains(collision.gameObject))
                m_detectedEnemies.Add(collision.gameObject);

            //enemeies are in sight (true) if there are more than 0 enemies on the list
            m_enemyInSight = m_detectedEnemies.Count > 0;

            m_enemyPos = collision.transform;
        }
    }

    //when enemy exits the crop's detection circle
    private void OnTriggerExit2D(Collider2D collision)
    {
        //so that it doesn't collide with the enemy's detection circle
        if (collision.isTrigger)
            return;

        int collisionLayer = collision.gameObject.layer;

        if (collisionLayer == LayerMask.NameToLayer("Player"))
            return;

        if (collisionLayer == LayerMask.NameToLayer("EnemyBody"))
        {
            //if the enemy leaving collision is on the list, it will be removed from the list
            if (m_detectedEnemies.Contains(collision.gameObject))
                m_detectedEnemies.Remove(collision.gameObject);

            //enemies in sight is false when there are 0 enemies left
            //not only when one leaves the collision
            m_enemyInSight = m_detectedEnemies.Count > 0;

            //if there are multiple and one leaves this makes sure that
            //the crop still fires at an enemy still in the collision
            if (m_detectedEnemies.Count > 0)
                m_enemyPos = m_detectedEnemies[0].transform;
            else
                m_enemyPos = null;
        }
    }
}

public enum CropStates
{
    Idle, Attack
};