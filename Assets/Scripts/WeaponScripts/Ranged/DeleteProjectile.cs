using UnityEngine;

public class DeleteProjectile : MonoBehaviour
{
    [Header("Destroy Projectile Parameters")]
    //The length of time before a projectile is
    //destroyed without collision
    [SerializeField] private float m_lifeTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, m_lifeTime);
    }
}
