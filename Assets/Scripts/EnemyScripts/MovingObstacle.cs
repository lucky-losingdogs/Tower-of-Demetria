using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform m_startWaypoint;
    [SerializeField] private Transform m_endWaypoint;
    [SerializeField] private float m_speed = 5;

    private Transform m_target;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_target = m_startWaypoint;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingObstacleWaypoint"))
        {
            ChangeTarget();
        }
    }

    void ChangeTarget()
    {
        if (m_target == m_startWaypoint)
        {
            m_target = m_endWaypoint;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (m_target == m_endWaypoint)
        {
            m_target = m_startWaypoint;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }
}
