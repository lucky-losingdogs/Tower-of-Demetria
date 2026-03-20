using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;

    public void AddButton(string name, Action onClickAction)
    {
        //clones the button prefab as a game object on the panel
        GameObject m_button = Instantiate(buttonPrefab, transform);

        //adds an on click event to the button
        m_button.GetComponent<Button>().onClick.AddListener(() => onClickAction());

        //sets the text on the button
        m_button.GetComponentInChildren<TMPro.TMP_Text>().text = name;
    }

    public void Toggle(bool val)
    {
        //destroys any other buttons that might still be toggled
        //before activating the new set of buttons
        if (val == true)
            RemoveOldButtons();

        gameObject.SetActive(val);
    }

    public void RemoveOldButtons()
    {
        foreach (Transform transformChildObjects in transform)
        {
            Destroy(transformChildObjects.gameObject);
        }
    }
}
