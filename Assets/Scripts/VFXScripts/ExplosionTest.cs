using System.Collections;
using UnityEngine;

public class ExploslionTest : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] private float m_explosionFrequency = 0.1f;
    [SerializeField, Range(0.5f, 4)] private float m_explosionRadius = 0.5f;

    private void Awake()
    {
        StartCoroutine(ExplosionSpawner());
    }

    private IEnumerator ExplosionSpawner()
    {
        while (true)
        {
            //wait
            yield return new WaitForSeconds(m_explosionFrequency);

            //find position to spawn explosion
            Vector3 explosionPos = transform.position;
            explosionPos += (Vector3)UnityEngine.Random.insideUnitCircle.normalized * m_explosionRadius;

            //spawn explosion
            VFXManager.CreateExplosion(explosionPos);
        }
    }
}
