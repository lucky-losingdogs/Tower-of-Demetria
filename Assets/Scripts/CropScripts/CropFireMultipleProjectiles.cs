using UnityEngine;

public class CropFireMultipleProjectiles : CropFireProjectile
{
    private float attackTimeout = 0f;
    private Vector2 m_fireDirection02;
    private Vector2 m_fireDirection03;

    public void FireProjectiles(Transform enemyPos)
    {
        if (!(ApplyDamage.CooldownFinished(attackTimeout)))
            return;

        m_firePoint = gameObject.transform.position;

        if (enemyPos != null)
        {
            m_fireDirection = ((Vector2)enemyPos.position - (Vector2)transform.position).normalized;
            m_fireDirection02 = Quaternion.Euler(0, 0, 45) * m_fireDirection;
            m_fireDirection03 = Quaternion.Euler(0, 0, -45) * m_fireDirection;
        }


        GameObject spawnedProjectile01 = Instantiate(m_projectilePrefab, m_firePoint, Quaternion.identity);
        GameObject spawnedProjectile02 = Instantiate(m_projectilePrefab, m_firePoint, Quaternion.identity);
        GameObject spawnedProjectile03 = Instantiate(m_projectilePrefab, m_firePoint, Quaternion.identity);

        AddForceToProjectiles(spawnedProjectile01, m_fireDirection);
        AddForceToProjectiles(spawnedProjectile02, m_fireDirection02);
        AddForceToProjectiles(spawnedProjectile03, m_fireDirection03);
    }

    private void AddForceToProjectiles(GameObject spawnedProjectile, Vector2 fireDirection)
    {
        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (projectileRB != null)
        {
            //adds force once
            //fire direction is normalised so speed is consistent diagonally
            projectileRB.AddForce(fireDirection * m_projectileSpeed, ForceMode2D.Impulse);

            attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
        }
    }
}
