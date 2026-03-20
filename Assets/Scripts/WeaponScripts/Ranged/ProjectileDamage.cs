using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    //reference to the particle that plays with the
    //projectile collides with something
    [SerializeField] private ParticleSystem m_projectileHitPrefab;
    [SerializeField] private ProjectileOwner m_projectileOwner;

    [SerializeField] private float m_damage = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        int collisionLayer = collision.gameObject.layer;

        int ignoreLayer = LayerMask.NameToLayer("DetectionSensor");
        if (gameObject.layer == ignoreLayer || collision.gameObject.layer == ignoreLayer)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
            return; // optional, safe to exit early
        }

        if (m_projectileOwner == ProjectileOwner.Player && (collisionLayer == LayerMask.NameToLayer("Player") || (collisionLayer == LayerMask.NameToLayer("Crop"))))
            return;

        if (m_projectileOwner == ProjectileOwner.Enemy && collisionLayer == LayerMask.NameToLayer("EnemyBody"))
            return;

        if (m_projectileOwner == ProjectileOwner.Player && collisionLayer == LayerMask.NameToLayer("EnemyBody"))
        {
            ApplyDamage.ApplyDmg(collision.gameObject, m_damage);
        }

        if (m_projectileOwner == ProjectileOwner.Enemy && collisionLayer == LayerMask.NameToLayer("Player"))
        {
            ApplyDamage.ApplyDmg(collision.gameObject, m_damage);
        }

        //gets the contact point of where the projectile collided
        Vector2 m_contactPoint = collision.ClosestPoint(transform.position);

        Destroy(gameObject);

        SpawnImpactEffect(m_contactPoint, Vector2.zero);
    }

    //parameters: projectile's point of contact and surface normal at contact point
    private void SpawnImpactEffect(Vector2 position, Vector2 normal)
    {
        //spawns the particles at the projectile's contact point
        //and with the rotation opposite the direction of the projectile
        ParticleSystem hitParticle = Instantiate(m_projectileHitPrefab, position, Quaternion.LookRotation(normal));

        //the particle effects lifetime is its duration and start lifetime
        float m_partLifetime = hitParticle.main.duration + hitParticle.main.startLifetime.constantMax;

        //destroys the particle effect after its lifetime has passed
        Destroy(hitParticle.gameObject, m_partLifetime);
    }
}

public enum ProjectileOwner
{
    Player, Enemy
}