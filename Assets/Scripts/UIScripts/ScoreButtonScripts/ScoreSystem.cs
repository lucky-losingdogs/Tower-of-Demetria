using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int m_score;

    public void AddScore(int scoreToAdd)
    {
        //increment score by given amount
        m_score += scoreToAdd;
    }
}
