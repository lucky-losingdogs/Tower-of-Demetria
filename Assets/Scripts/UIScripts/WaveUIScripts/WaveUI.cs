using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_waveText;
    [SerializeField] private EnemyWaves m_enemyWavesSystem;

    private void Start()
    {
        if (m_enemyWavesSystem == null)
        {
            Debug.Log("wave system is not assigned to ui");
            return;
        }

        //subscribe handler functions to events
        m_enemyWavesSystem.OnWaveIntermission += HandleWaveIntermission;
        m_enemyWavesSystem.OnNextWave += HandleNextWave;
    }

    //update ui text to show the current wave during a wave
    private void HandleNextWave()
    {
        if (m_enemyWavesSystem.m_waveActive == true)
            m_waveText.text = $"Wave: {m_enemyWavesSystem.m_currentWaveNum}";
    }

    //update ui text to show the countdown until the next wave
    private void HandleWaveIntermission()
    {
        if (m_enemyWavesSystem.m_waveActive == false)
            m_waveText.text = $"Next Wave In: {m_enemyWavesSystem.m_waveIntermission}";
    }
}
