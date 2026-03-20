using UnityEngine;

public class GoblinSpecialAttack : MonoBehaviour
{
    [SerializeField] private float m_attackCooldown = 5f;

    [SerializeField] private float m_projectileSpeed = 10f;

    [SerializeField] private GameObject m_projectilePrefab;

    Vector2 m_fireDirection;

    Vector2 m_firePoint;

    private float m_attackTimeout = 0f;

    private bool m_playerInSight = false;

    private Transform m_playerPos;

    private EnemyStates m_enemyStates = EnemyStates.Idle;


    private void Update()
    {
        if (m_playerInSight)
        {
            m_enemyStates = EnemyStates.Attack;
        }

        if (m_enemyStates == EnemyStates.Attack)
        {
            FireProjectile();
        }
    }

    //when player enters the enemy's detection circle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //so that it doesn't collide with the enemy's detection circle
        if (collision.isTrigger)
            return;

        if (collision.CompareTag("Player"))
        {
            m_playerPos = collision.transform;
            m_playerInSight = true;
        }
    }

    //when player exits the enemy's detection circle
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        if (collision.CompareTag("Player"))
        {
            m_playerInSight = false;
            m_playerPos = null;
        }
    }

    private void FireProjectile()
    {
        if (!(ApplyDamage.CooldownFinished(m_attackTimeout)))
            return;

        m_firePoint = gameObject.transform.position;

        if (m_playerPos != null)
        {
            m_fireDirection = ((Vector2)m_playerPos.position - (Vector2)transform.position).normalized;
        }


        GameObject spawnedProjectile = Instantiate(m_projectilePrefab, m_firePoint, Quaternion.identity);

        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (projectileRB != null)
        {
            //adds force once
            //fire direction is normalised so speed is consistent diagonally
            projectileRB.AddForce(m_fireDirection * m_projectileSpeed, ForceMode2D.Impulse);

            m_attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
        }
    }
}

public enum EnemyStates
{
    Idle, ChasePlayer, Attack
};