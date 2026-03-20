using UnityEngine;

public class SpawnMonsterDrop : MonoBehaviour
{
    [SerializeField] private GameObject m_monsterDropPrefab;
    [SerializeField] private HP hp;

    private void Start()
    {
        hp.OnEnemyDeath += MonsterDrop;
    }

    private void MonsterDrop()
    {
        Instantiate(m_monsterDropPrefab, transform.position, Quaternion.identity);
    }
}
