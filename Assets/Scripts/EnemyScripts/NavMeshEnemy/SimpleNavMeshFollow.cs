using UnityEngine;
using UnityEngine.AI;

public class SimpleNavMeshFollow : MonoBehaviour
{
    [SerializeField] private Transform[] m_targets;
    private Transform m_target;

    //reference to the navmesh component which
    //controls the navigation of the agent/enemy
    private NavMeshAgent m_agent;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        //make the enemy's target random between the 4 end points
        int randomTargetIndex = Random.Range(0, m_targets.Length);
        m_target = m_targets[randomTargetIndex];

        if (m_target == null)
        {
            Debug.Log("enemy target not set");
            return;
        }

        m_agent.SetDestination(m_target.position);
    }
}
