using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuSystem pauseMenuSystem;

    private InputAction m_pauseAction;

    private void Awake()
    {
        //binds the p key to inventoryAction
        m_pauseAction = InputSystem.actions.FindAction("Pause");
    }
    public void Update()
    {
        if (m_pauseAction.WasPressedThisFrame())
        {
            MenuSorting.CloseAllMenus();
            pauseMenuSystem.TogglePausePanel();
        }
    }
}
