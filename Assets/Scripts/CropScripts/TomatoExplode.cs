using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoExplode : MonoBehaviour
{
    private float m_damage = 100;
    
    public void TomatoAttack(List<GameObject> detectedEnemies)
    {
        StartCoroutine(Countdown(detectedEnemies));
    }

    //after a 3s countdown, destroy all the enemies within it's collision circle
    private IEnumerator Countdown(List<GameObject> detectedEnemies)
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < detectedEnemies.Count; i++)
        {
            ApplyDamage.ApplyDmg(detectedEnemies[i], m_damage);
        }
        DestroyTomato();
    }

    //destroy the tomato after it has attacked once
    private void DestroyTomato()
    {
        //hides the tomato
        gameObject.SetActive(false);

        //creates an explosion vfx
        VFXManager.CreateExplosion(transform.position);

        //destroys the tomato game object after the explosion
        Destroy(gameObject);
    }
}
