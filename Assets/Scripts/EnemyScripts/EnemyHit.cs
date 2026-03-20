using UnityEngine;

public class EnemyHit : MonoBehaviour
{
   [SerializeField] private Animator m_animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            return;


        //if enemy does not collide with another enemy tag and does collide with the player tag
        //bool isHit = true and changes to the enemy damaged animation
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            //changes to the enemy damaged animation
            m_animator.SetTrigger("IsHit");
        }
    }

    
    public void TakeDamage(float dmg)
    {
        HP health = GetComponent<HP>();
        if (health != null)
        {
            health.ReduceHP(dmg);
        }
    }
}