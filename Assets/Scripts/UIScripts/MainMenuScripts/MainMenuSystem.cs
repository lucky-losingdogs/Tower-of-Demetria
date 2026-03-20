using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_menuPanel;
    [SerializeField] private GameObject m_controlsPanel;
    [SerializeField] private GameObject m_instrPanel;
    [SerializeField] private MenuLoading m_menuLoading;

    private bool m_controlsPanelOpen = false;
    private bool m_instrPanelOpen = false;

    private void Start()
    {
        //ensures that the control panel is not active on start
        m_controlsPanel.SetActive(false);

        //async loads level 1 on start with coroutine
        StartCoroutine(m_menuLoading.LoadLvlAsync(1));
    }

    //opens game into the first scene
    public void PlayGame()
    {
        m_menuLoading.LoadLevel();
    }

    //quits the game
    public void QuitGame()
    {
        m_menuLoading.QuitGame();
    }

    public void ToggleControlsPanel()
    {
        //if controls open, close it and open menu
        if (m_controlsPanelOpen)
        {
            m_controlsPanel.SetActive(false);
            m_menuPanel.SetActive(true);
        }
        //if controls not open, open it and close meu
        else
        {
            m_controlsPanel.SetActive(true);
            m_menuPanel.SetActive(false);
        }
        
        //changes bool to opposite state every time toggled
        m_controlsPanelOpen = !m_controlsPanelOpen;
    }

    public void ToggleInstructionsPanel()
    {
        //if instructions open, close it and open menu
        if (m_instrPanelOpen)
        {
            m_instrPanel.SetActive(false);
            m_menuPanel.SetActive(true);
        }
        //if instructions not open, open it and close meu
        else
        {
            m_instrPanel.SetActive(true);
            m_menuPanel.SetActive(false);
        }

        //changes bool to opposite state every time toggled
        m_instrPanelOpen = !m_instrPanelOpen;
    }
}
