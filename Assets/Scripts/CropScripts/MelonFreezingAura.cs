using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MelonFreezingAura : MonoBehaviour
{
    private List<NavMeshAgent> m_frozenAgents = new List<NavMeshAgent>();


    private void OnTriggerStay2D(Collider2D collision)
    {
        //so it's not colliding with the enemy's trigger collider, and only its actual body
        if (collision.isTrigger)
            return;

        
        //if the thing that's colliding has the tag "EnemyBody"
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
        {
            //get the nav mesh agent component on the enemy
            NavMeshAgent agent = GetAgent(collision);

            //if the agent isn't on the frozen agents list, add it and stop it's movement
            if (agent != null && !m_frozenAgents.Contains(agent))
            {
                agent.isStopped = true;
                m_frozenAgents.Add(agent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //so it's not colliding with the crop's trigger collider, and only its actual body
        if (collision.isTrigger)
            return;

        //if the thing that's colliding has the tag "EnemyBody"
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
        {
            NavMeshAgent agent = GetAgent(collision);

            //if the agent is on the frozen agents list, remove it and restore it's movement
            if (agent != null && m_frozenAgents.Contains(agent))
            {
                agent.isStopped = false;
                m_frozenAgents.Remove(agent);
            }
        }
    }

    //sets the crop and crop controller variables
    private NavMeshAgent GetAgent(Collider2D collision)
    {
        return collision.GetComponentInParent<NavMeshAgent>();
    }
}