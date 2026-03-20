using UnityEngine;

public class RangedWeapon : WeaponBase
{
    private GameObject m_projectilePrefab;
    private float m_projectileSpeed;

    public void InitialiseRanged(GameObject projectile, float projectileSpeed)
    {
        m_projectilePrefab = projectile;
        m_projectileSpeed = projectileSpeed;
    }

    public override void Attack()
    {
        if (m_projectilePrefab == null)
        {
            Debug.LogError("Ranged weapon missing prefab");
            return;
        }

        Fire();
    }

    public void Fire()
    {
        
        if (owner == null)
        {
            Debug.LogError("Owner missing?");
            return;
        }

        //fire direction is the last direction player is facing
        Vector2 fireDirection = owner.m_lastDirection;
        if (fireDirection == Vector2.zero)
        {
            fireDirection = Vector2.down;
        }

        Vector2 spawnPos = owner.transform.position;


        //creates the projectiles at runtime
        //paramaters: reference to the object we want to spawn,
        //location where the object will be spawned,
        //rotation at which the object will be spawned (no rotation for this one)
        GameObject spawnedProjectile = Instantiate(m_projectilePrefab, spawnPos, Quaternion.identity);

        //checks if the projectile has a rigidbody
        Rigidbody2D projectileRB = spawnedProjectile.GetComponent<Rigidbody2D>();
        if (projectileRB != null)
        {
            //adds force once
            //fire direction is normalised so speed is consistent diagonally
            projectileRB.AddForce(fireDirection.normalized * m_projectileSpeed, ForceMode2D.Impulse);
        }

    }
}