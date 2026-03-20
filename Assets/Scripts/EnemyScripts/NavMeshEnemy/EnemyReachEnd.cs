using UnityEngine;

public class EnemyReachEnd : MonoBehaviour
{
    [SerializeField] private float m_damage = 25;
    [SerializeField] private float m_attackCooldown = 5;
    [SerializeField] private Animator m_animator;
    private float m_attackTimeout;
    private GameObject m_player;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    
    //when the enemy hits the end waypoint, it damages the player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyEndPoint"))
        {
            if (m_player == null)
                return;

            EnemyAttack();
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
