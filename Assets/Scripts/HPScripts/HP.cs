using System;
using System.Collections;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    [SerializeField] public float m_maxHP = 100f;
    [SerializeField] public float m_currentHP;
    [SerializeField] private HPType m_hpType;
    [SerializeField] private AudioClip m_deathAudioClip;
    [SerializeField] private Animator m_animator;
    [SerializeField] private GameObject m_hpVignette;

    private bool isDead = false;

    private SpriteRenderer m_spriteRenderer;
    Color m_originalColor;

    public event Action OnHealthChange;
    public event Action OnEnemyDeath;

    private void Awake()
    {
        //on start, currentHP will be the same as maxHP
        ResetHP();
    }

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_originalColor = m_spriteRenderer.color;
    }

    public void ResetHP()
    {
        m_currentHP = m_maxHP;
        UpdatePlayerUI();
    }

    public void ReduceHP(float dmg)
    {
        //if damage is 0 or less return
        if (dmg <= 0)
            return;

        if (isDead)
            return;

        //subtract damage from currentHP
        m_currentHP -= dmg;

        if (m_hpType == HPType.Player)
        {
            UpdatePlayerUI();
            StartCoroutine(PlayerFlash());
            SetVignette();
        }
            

        //if currentHP is 0 or less call death function
        if (m_currentHP <= 0)
        {
            Die();
            return;
        }
    }

    public void AddHP(float hpBoost)
    {
        //if currentHP is already the maxHP return
        if (m_currentHP == m_maxHP)
            return;

        if (isDead)
            return;

        //change currentHP to the currentHP + the boost if its less than the maxHP
        //or the maxHP if currentHP ends up larger than maxHP
        m_currentHP = Mathf.Min(m_currentHP + hpBoost, m_maxHP);

        if (m_hpType == HPType.Player)
        {
            UpdatePlayerUI();
            SetVignette();
        }
    }

    public void Die()
    {
        SFXManager.PlayAudio(m_deathAudioClip);

        if (m_hpType == HPType.Player)
        {
            Debug.Log("Died");
            GameState.m_gameRunning = false;
            m_hpVignette.SetActive(false);
            SceneManager.LoadScene(2);
        }
        else
        {
            EnemyWaves.m_enemyCount--;
            isDead = true;

            StartCoroutine(DestroyEnemy());
        }
    }

    public void UpdatePlayerUI()
    {
        if (m_hpType == HPType.Player)
        {
            //invoke event to update ui to match hp
            OnHealthChange?.Invoke();
        }
    }

    private IEnumerator PlayerFlash()
    {
        m_spriteRenderer.color = new Color(2f, 2f, 2f, 5f);

        yield return new WaitForSeconds(0.1f);

        m_spriteRenderer.color = m_originalColor;
    }

    private void SetVignette()
    {
        if (m_currentHP <= 25)
        {
            m_hpVignette.SetActive(true);
        }
        else
        {
            m_hpVignette.SetActive(false);
        }
    }

    private IEnumerator DestroyEnemy()
    {
        m_animator.SetBool("Dead", true);

        yield return new WaitForSeconds(1);

        //hides the enemy parent, which also prevents any additional child behaviour continuing
        gameObject.transform.parent.gameObject.SetActive(false);

        //creates an explosion vfx
        VFXManager.CreateExplosion02(transform.position);

        OnEnemyDeath?.Invoke();

        //destroys the game object after the explosion
        Destroy(gameObject.transform.parent.gameObject);
    }

    enum HPType
    {
        Player, Enemy
    };
}
