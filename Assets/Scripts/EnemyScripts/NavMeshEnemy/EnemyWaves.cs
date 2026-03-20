using System.Collections;
using UnityEngine;
using System;

public struct Waves
{
    public int m_waveIntermissionTime;
    public float m_spawnRate;
    public int[] m_enemyIndex;
}

public class EnemyWaves : MonoBehaviour
{
    [SerializeField] private Transform[] m_enemyStartPoint;
    [SerializeField] private GameObject[] m_enemyArray;

    public event Action OnWaveIntermission;
    public event Action OnNextWave;

    public static int m_enemyCount = 0;
    public int m_currentWaveNum = 0;
    public int m_waveIntermission;
    public bool m_waveActive;
    private Transform m_enemyParent;

    private bool m_audioToggle;

    private void Start()
    {
        if (m_enemyArray == null)
        {
            Debug.Log("Enemy array is null");
            return;
        }
        else if (m_enemyStartPoint == null)
        {
            Debug.Log("Enemy start point is null");
            return;
        }

        m_enemyParent = GameObject.Find("EnemyParent").transform;

        StartCoroutine(RunWaves());
    }

    private Waves GenerateWave(int waveNumber)
    {
        Waves wave = new Waves();

        int baseEnemyNum = 1;
        int enemiesInWave = baseEnemyNum + (waveNumber * 2);
        wave.m_enemyIndex = new int[enemiesInWave];

        //determine the max/hardest enemy index allowed in this wave
        int maxEnemyIndex = Mathf.Clamp((waveNumber * m_enemyArray.Length) / 8, 0, m_enemyArray.Length - 1);
        for (int i = 0; i < wave.m_enemyIndex.Length; i++)
        {
            //pick a random enemy type from 0 to maxEnemyIndex
            wave.m_enemyIndex[i] = UnityEngine.Random.Range(0, maxEnemyIndex + 1);
        }

        //guarantees that a boss enemy will be in every 10th wave
        if ((waveNumber % 10) == 0)
        {
            wave.m_enemyIndex[wave.m_enemyIndex.Length - 1] = 5;
        }

        wave.m_waveIntermissionTime = Mathf.Max(20, 20 - waveNumber * 2);
        wave.m_spawnRate = Mathf.Clamp(5 - waveNumber * 0.2f, 1f, 5f);


        return wave;
    }

    private IEnumerator RunWaves()
    {
        SwitchBGAudio();

        //iterates through the waves
        while (GameState.m_gameRunning)
        {
            m_currentWaveNum++;

            //creates the next wave
            Waves nextWave = GenerateWave(m_currentWaveNum);
            

            //set the wave intermission var to the generated waves' intermission time
            //for the ui to access
            m_waveIntermission = nextWave.m_waveIntermissionTime;

            //start coroutine that updates the ui intermission timer every second
            StartCoroutine(IntermissionCountdown());

            yield return new WaitForSeconds(nextWave.m_waveIntermissionTime);

            SFXManager.PlayBackgroundBattle();
            m_waveActive = true;
            OnNextWave?.Invoke();
            yield return StartCoroutine(EnemyWave(nextWave));


            Debug.Log($"enemy count: {m_enemyCount}");

            //waits until all the enemies have died
            //before starting the intermission for the next wave
            yield return new WaitUntil(WaveOverCheck);
            m_waveActive = false;
            SwitchBGAudio();
        }
    }

    //coroutine that updates the ui text every second
    //to match the intermission timer
    private IEnumerator IntermissionCountdown()
    {
        while (m_waveIntermission > 0 && !m_waveActive)
        {
            OnWaveIntermission?.Invoke();
            yield return new WaitForSeconds(1);
            m_waveIntermission--;
        }
    }


    //iterates through each enemy in a wave and spawns them at the start point
    private IEnumerator EnemyWave(Waves wave)
    {
        for (int i = 0; i < wave.m_enemyIndex.Length; i++)
        {
            yield return new WaitForSeconds(wave.m_spawnRate);
            if (m_enemyArray[wave.m_enemyIndex[i]] != null)
            {
                //chooses a random index for the enemy spawn point to be random
                int randomStartIndex = UnityEngine.Random.Range(0, m_enemyStartPoint.Length);
                Instantiate(m_enemyArray[wave.m_enemyIndex[i]], m_enemyStartPoint[randomStartIndex].position, Quaternion.identity, m_enemyParent);
                m_enemyCount++;
            }
        }

        Debug.Log("Finished spawning enemies this wave");
    }

    //check if the enemy count has reached 0
    private bool WaveOverCheck()
    {
        if (m_enemyCount == 0)
            return true;
        else return false;
    }

    //check if the enemy count has reached 1
    private bool WaveStartCheck()
    {
        if (m_enemyCount == 0)
            return false;
        else return true;
    }

    public GameObject MatchEnemyID(EnemyType enemyID)
    {
        for (int i = 0; i < m_enemyArray.Length; i++)
        {
            NavEnemyController m_navEnemyController = m_enemyArray[i].GetComponent<NavEnemyController>();

            if (m_navEnemyController.enemyType == enemyID)
            {
                return m_enemyArray[i];
            }

        }

        return null;
    }

    private void SwitchBGAudio()
    {
        if (m_audioToggle)
        {
            SFXManager.PlayBackground01();
        }
        else
        {
            SFXManager.PlayBackground02();
        }
        m_audioToggle = !m_audioToggle;
    }
}