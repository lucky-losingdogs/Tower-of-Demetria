using System.Collections.Generic;
using UnityEngine;

public class CropMeleeAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider2D m_boxCollider;
    [SerializeField] private float m_attackCooldown;
    [SerializeField] private float m_damage;

    private float m_attackTimeout = 0f;


    public void MeleeAttack(List<GameObject> detectedEnemies)
    {
        if (!(ApplyDamage.CooldownFinished(m_attackTimeout)))
            return;

        for (int i = 0; i < detectedEnemies.Count; i++)
        {
            //spawns slash vfx
            VFXManager.CreateSlash(detectedEnemies[i].transform.position);
            ApplyDamage.ApplyDmg(detectedEnemies[i], m_damage);
        }

        m_attackTimeout = ApplyDamage.StartCooldown(m_attackCooldown);
    }
}
