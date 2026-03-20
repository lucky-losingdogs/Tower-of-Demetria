using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuLoading : MonoBehaviour
{
    public bool m_loadLevel = false;

    public IEnumerator LoadLvlAsync(int level)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        //doesn't load the level immediately
        asyncLoad.allowSceneActivation = false;

        //returns null until the level has finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;

            //loads level is level is loaded to 0.9% and start button is pressed
            if (asyncLoad.progress >= 0.9f && m_loadLevel)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
    }

    //loads scene
    public void LoadLevel()
    {
        GameState.m_gameRunning = true;
        m_loadLevel = true;
    }

    //quits the game
    public void QuitGame()
    {
        GameState.m_gameRunning = false;
        Debug.Log("Quit game pressed");
        Application.Quit();
    }
}
