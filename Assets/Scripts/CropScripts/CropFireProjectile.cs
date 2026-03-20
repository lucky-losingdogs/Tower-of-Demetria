using UnityEngine;

public class CropFireProjectile : MonoBehaviour
{
    #region Variables

    [SerializeField] protected GameObject m_projectilePrefab;

    [SerializeField] protected float m_attackCooldown = 2f;

    [SerializeField] protected float m_projectileSpeed = 15f;

    private float m_attackTimeout = 0f;

    protected Vector2 m_firePoint;

    protected Vector2 m_fireDirection;

    #endregion


    //if an enemy collides with the crop's detection circle, it fires a projectile at it.
    public void FireProjectile(Transform enemyPos)
    {
        if (!(ApplyDamage.CooldownFinished(m_attackTimeout)))
            return;

        m_firePoint = gameObject.transform.position;

        if (enemyPos != null)
        {
            m_fireDirection = ((Vector2)enemyPos.position - (Vector2)transform.position).normalized;
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
