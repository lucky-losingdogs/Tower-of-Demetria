using UnityEngine;

public class NavEnemyController : MonoBehaviour
{
    #region Variables

    private float m_attackTimeout = 0f;

    //reference to the player's game object
    private GameObject m_player;

    [SerializeField] private float m_damage = 10;
    [SerializeField] private float m_attackCooldown = 2;
    [SerializeField] private Animator m_animator;
    public EnemyType enemyType;

    #endregion

    //repeatedly damage the player with cooldown
    //when the player collides with the enemy's main body
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_player = collision.gameObject;

            if (m_player == null)
                return;

            EnemyAttack();
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

    //check if the cooldown has finished, apply dmg to player, trigger the attacking anim, restart cooldown
    private void EnemyAttack()
    {
        if (ApplyDamage.CooldownFinished(m_attackTimeout))
        {
            ApplyDamage.ApplyDmg(m_player, m_damage);
            m_animator.SetTrigger("Attacking");
            m_attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
        }
    }
}

public enum EnemyType
{
    Slime, Wolf, Bee, Goblin, MushBoss, Null
}
