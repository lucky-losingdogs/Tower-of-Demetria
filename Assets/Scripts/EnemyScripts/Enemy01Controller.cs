using UnityEngine;

public class Enemy01Controller : MonoBehaviour
{
    #region Variables

    //enemy speed
    [SerializeField] private float m_speed = 1;

    //distance that the enemy will leave between themself and the player
    [SerializeField] private float m_stoppingDistance = 2.5f;

    //damage the enemy applies to the player on collision
    [SerializeField] private float m_damage = 10f;

    //time between enemy attacks while still colliding
    [SerializeField] private float m_attackCooldown = 2f;

    [SerializeField] private EnemyDetection m_enemyDetection;

    private float m_attackTimeout = 0f;

    //reference to the player's location
    public Transform m_playerPos;

    //reference to the player's game object
    private GameObject m_player;

    //reference to the enemy's rigidbody component
    Rigidbody2D m_rigidbody;

    //if the player is within the enemy's detection circle
    bool m_playerInSight;

    #endregion


    private EnemyStates m_enemyStates = EnemyStates.Idle;


    private void Start()
    {
        TopDownCharacterController player = FindObjectOfType<TopDownCharacterController>();
        if (player != null)
            m_playerPos = player.transform;

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_playerInSight = m_enemyDetection.m_playerInSight;

        switch (m_enemyStates)
        {
            case EnemyStates.Idle:
                {
                    if (m_rigidbody != null)
                        m_rigidbody.linearVelocity = Vector2.zero;
                    break;
                }
            case EnemyStates.ChasePlayer:
                {
                    transform.position = Vector2.MoveTowards(transform.position, m_playerPos.position, m_speed * Time.deltaTime);
                    break;
                }
            case EnemyStates.Attack:
                {
                    if (m_player != null)
                        return;

                    if (ApplyDamage.CooldownFinished(m_attackTimeout))
                    {
                        ApplyDamage.ApplyDmg(m_player, m_damage);
                        m_attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
                    }

                    break;
                }
        }

        //state transitions based on detection and distance
        if (m_playerInSight)
        {
            m_enemyStates = EnemyStates.ChasePlayer;
        }
        else if (Vector2.Distance(transform.position, m_playerPos.position) <= m_stoppingDistance)
        {
            m_enemyStates = EnemyStates.Attack;
        }
        else
        {
            m_enemyStates = EnemyStates.Idle;
        }
    }

    //when the player first collides with the enemy's main body apply damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_player = collision.gameObject;
        }
    }

    //if collision persists, repeatedly damage the player with cooldown
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_player = collision.gameObject;
        }
    }

    //when the player stops colliding with the enemy's main body,
    //clear reference to player collision
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_player == collision.gameObject)
            {
                m_player = null;
            }
        }
    }
}
