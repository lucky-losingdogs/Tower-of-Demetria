using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    //reference to the ui score text
    public TMPro.TextMeshProUGUI m_scoreText;

    //reference to the score system
    private ScoreSystem m_scoreSystem;

    //on startup, gets reference to ScoreSystem script
    private void Awake()
    {
        m_scoreSystem = GetComponent<ScoreSystem>();
    }

    private void Update()
    {
        //change the ui score text
        m_scoreText.text = $"Score: {m_scoreSystem.m_score}";
    }
}
