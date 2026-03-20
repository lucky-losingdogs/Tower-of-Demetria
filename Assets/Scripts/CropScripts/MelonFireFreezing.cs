using UnityEngine;

public class MelonFireFreezing : MonoBehaviour
{
    [SerializeField] private GameObject m_freezingAura;
    [SerializeField] private float m_attackCooldown;

    private float m_attackTimeout = 0;

    public void MoveFreezingAura(Transform enemyPos)
    {
        if (!(ApplyDamage.CooldownFinished(m_attackTimeout)))
            return;

        if (enemyPos != null)
        {
            m_freezingAura.transform.position = enemyPos.position;
        }

        m_attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
    }

}
