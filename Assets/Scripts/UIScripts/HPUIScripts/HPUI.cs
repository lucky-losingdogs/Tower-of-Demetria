using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_HPText;
    [SerializeField] private Image m_healthBar;
    [SerializeField] private HP m_hpSystem;
    [SerializeField] private Animator m_hpBarAnim;

    private void Start()
    {
        if (m_hpSystem == null)
        {
            Debug.Log("HP system is null");
            return;
        }

        //subscribing the function to the OnHealthChange event
        //function is invoked in HP when health is increased or decreased
        m_hpSystem.OnHealthChange += HandleHealthChanged;

        //initial hp bar update to match pre-existing hp change
        HandleHealthChanged();
    }

    private void HandleHealthChanged()
    {
        //change the ui score text
        m_HPText.text = $"HP: {m_hpSystem.m_currentHP}";

        //calculates the ratio between the current hp and the max hp
        float healthRatio = m_hpSystem.m_currentHP / m_hpSystem.m_maxHP;
        //Debug.Log("health ratio:" + healthRatio);

        if (m_hpBarAnim != null)
        {
            m_hpBarAnim.SetTrigger("ChangeHP");
        }

        //checks if the image tyoe is the Filled type
        if (m_healthBar.type == Image.Type.Filled)
        {
            //updates the image fill amount based on the hp ratio
            m_healthBar.fillAmount = healthRatio;
        }
    }
}