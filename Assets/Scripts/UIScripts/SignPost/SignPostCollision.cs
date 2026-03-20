using UnityEngine;

public class SignPostCollision : MonoBehaviour
{
    [SerializeField] private GameObject m_signUI;
    private bool signActive = false;

    private void Awake()
    {
        m_signUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (m_signUI == null)
            return;

        if (!signActive)
        {
            m_signUI.SetActive(true);
            signActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (m_signUI == null)
            return;

        if (signActive)
        {
            m_signUI.SetActive(false);
            signActive = false;
        }
    }
}
