using UnityEngine;
using UnityEngine.UI;

public class SignPostButtons : MonoBehaviour
{
    [SerializeField] GameObject m_image01; 
    [SerializeField] GameObject m_image02;
    [SerializeField] private GameObject m_signUI;

    private bool image01Active = true;
    private bool image02Active = false;

    private void Awake()
    {
        m_image01.SetActive(true);
        m_image02.SetActive(false);
    }

    public void LeftSignBtn()
    {
        if (!image01Active)
        {
            m_image01.SetActive(true);
            m_image02.SetActive(false);

            image01Active = true;
            image02Active = false;
        }
    }

    public void RightSignBtn()
    {
        if (!image02Active)
        {
            m_image01.SetActive(false);
            m_image02.SetActive(true);

            image01Active = false;
            image02Active = true;
        }
    }

    public void ExitButton()
    {
        m_signUI.SetActive(false);
    }
}
