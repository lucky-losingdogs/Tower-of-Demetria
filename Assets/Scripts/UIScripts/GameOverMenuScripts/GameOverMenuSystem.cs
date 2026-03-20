using UnityEngine;

public class GameOverMenuSystem : MonoBehaviour
{
    [SerializeField] MenuLoading m_menuLoading;

    private void Start()
    {
        //async loads level 1 on start with coroutine
        StartCoroutine(m_menuLoading.LoadLvlAsync(1));
    }

    public void TryAgain()
    {
        m_menuLoading.LoadLevel();
    }

    public void QuitGame()
    {
        m_menuLoading.QuitGame();
    }
}
