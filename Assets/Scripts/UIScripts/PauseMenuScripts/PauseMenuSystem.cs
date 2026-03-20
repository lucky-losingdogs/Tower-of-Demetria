using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PauseMenuSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_pausePanel;
    [SerializeField] private GameObject m_controlsPanel;
    [SerializeField] private TextMeshProUGUI m_saveText;
    [SerializeField] private TextMeshProUGUI m_loadText;

    [SerializeField] private AssignSaveData m_assignSaveData;

    private bool m_pausePanelOpen = false;
    private bool m_controlsPanelOpen = false;


    private void Awake()
    {
        //ensures the pause menu is not open on start
        m_pausePanel.SetActive(false);
        m_controlsPanel.SetActive(false);
    }

    public void MainMenuButton()
    {
        //returns time to normal
        Time.timeScale = 1.0f;

        //loads main menu when main menu button clicked
        SceneManager.LoadScene(0);
    }

    public void TogglePausePanel()
    {
        if (m_pausePanelOpen)
        {
            //if pause menu open, close it
            m_pausePanel.SetActive(false);
            
            //returns time to normal
            Time.timeScale = 1.0f;
            m_saveText.text = "Save";
            m_loadText.text = "Load";
        }
        else
        {
            //if pause menu closed, open it
            m_pausePanel.SetActive(true);
            
            //pauses time
            Time.timeScale = 0;
        }

        //changes bool to opposite state every time toggled
        m_pausePanelOpen = !m_pausePanelOpen;
    }

    public void ToggleControlsPanel()
    {
        //if controls open, close it and open menu
        if (m_controlsPanelOpen)
        {
            m_controlsPanel.SetActive(false);
            m_pausePanel.SetActive(true);
        }
        //if controls not open, open it and close meu
        else
        {
            m_controlsPanel.SetActive(true);
            m_pausePanel.SetActive(false);
        }

        //changes bool to opposite state every time toggled
        m_controlsPanelOpen = !m_controlsPanelOpen;
    }

    public void SaveButton()
    {
        SaveSystem.SavePlayer(m_assignSaveData.m_player);
        m_saveText.text = "Saved";
    }

    //get the saved data and update the player's current data with the saved data
    public void LoadSaveButton()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        m_assignSaveData.AssignLoadedData(data);
        m_loadText.text = "Loaded";
    }
}
