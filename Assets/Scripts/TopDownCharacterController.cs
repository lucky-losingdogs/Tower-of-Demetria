using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// A class to control the top-down character.
/// Implements the player controls for moving and shooting.
/// Updates the player animator so the character animates based on input.
/// </summary>
public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Variables

    //reference to the script generated based off the input maps
    //InputSystem_Actions m_actions;


    //The inputs that we need to retrieve from the input system.
    private InputAction m_moveAction;
    private InputAction m_attackAction;
    private InputAction m_rollAction;

    //The components that we need to edit to make the player move smoothly.
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;

    //The direction that the player is moving in.
    private Vector2 m_playerDirection;

    //the last direction that the player was facing.
    public Vector2 m_lastDirection { get; private set; }


    [Header("Movement Parameters")]
    //The speed at which the player moves
    [SerializeField] private float m_playerSpeed = 200f;
    //The maximum speed the player can move
    [SerializeField] private float m_playerMaxSpeed = 1000f;

    #endregion

    //reference to the equipped weapon component
    private EquipWeapon m_equippedWeapon;
    private float m_fireTimeout = 0;

    //roll timeouts and the time it takes to roll again
    private float m_rollTimeout = 0;
    private float m_rollCooldown = 1.0f;

    //reference to the inventory controller
    private InventoryController m_inventoryController;
    private GameObject[] m_menuUIs;
    [SerializeField] private AudioClip m_attackAudioClip;


    private void Awake()
    {
        //bind movement inputs to variables
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_rollAction = InputSystem.actions.FindAction("Roll");

        //m_actions = new InputSystem_Actions();

        //get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_equippedWeapon = GetComponent<EquipWeapon>();
        m_inventoryController = GetComponent<InventoryController>();
    }

    void Start()
    {
        //initialise any weapons equipped before runtime
        //in Start() to run after Awake() in m_equippedWeapon
        m_equippedWeapon.InitialiseCurrentWeapon();
        m_menuUIs = GameObject.FindGameObjectsWithTag("BlockingUI");
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc performance.
    /// This ensures that physics are calculated properly.
    /// </summary>
    private void FixedUpdate()
    {
        //clamp the speed to the maximum speed for if the speed has been changed in code.
        float speed = m_playerSpeed > m_playerMaxSpeed ? m_playerMaxSpeed : m_playerSpeed;

        //apply the movement to the character using the clamped speed value.
        m_rigidbody.linearVelocity = m_playerDirection * (speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// When the update loop is called, it runs every frame.
    /// Therefore, this will run more or less frequently depending on performance.
    /// Used to catch changes in variables or input.
    /// </summary>
    void Update()
    {
        // store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        m_playerDirection = m_moveAction.ReadValue<Vector2>();

        // ~~ handle animator ~~
        // Update the animator speed to ensure that we revert to idle if the player doesn't move.
        m_animator.SetFloat("Speed", m_playerDirection.magnitude);

        // If there is movement, set the directional values to ensure the character is facing the way they are moving.
        if (m_playerDirection.magnitude > 0)
        {
            m_animator.SetFloat("Horizontal", m_playerDirection.x);
            m_animator.SetFloat("Vertical", m_playerDirection.y);

            //checking if a roll is triggered and also if an amount
            //of time since the last roll has passed
            if (m_rollAction.IsPressed() && Time.time > m_rollTimeout)
            {
                m_rollTimeout = Time.time + m_rollCooldown;

                //increases player speed during roll
                m_playerSpeed *= 3;

                //changes char anim to rolling anim
                m_animator.SetTrigger("Rolling");

                //runs coroutine with timer to delay char speed returning to
                //normal after roll
                StartCoroutine(RollComplete());
            }

            //sets the last facing direction for projectile shooting
            m_lastDirection = m_playerDirection;
        }

        //check if an attack has been triggered.
        //Time.time returns the amount of time that has passed
        //once exceeding m_fireRate, Fire() is called again
        if (m_attackAction.IsPressed() && Time.time >= m_fireTimeout && !m_inventoryController.inventoryUI.isActiveAndEnabled)
        {
            if (!IsMenuOpen())
            {
                CameraShake.ShakeCam(0.5f);

                m_animator.SetTrigger("Melee");
                SFXManager.PlayAudio(m_attackAudioClip);

                //adds to the timer inbetween attacks
                m_fireTimeout = Time.time + m_equippedWeapon.m_currentWeaponComponent.m_attackRate;
                m_equippedWeapon.m_currentWeaponComponent?.Attack();
            }
        }
    }

    //timer to delay char speed returning to normal after roll
    private IEnumerator RollComplete()
    {
        yield return new WaitForSeconds(0.3f);
        m_playerSpeed /= 3;
    }

    private bool IsMenuOpen()
    {
        foreach (var menu in m_menuUIs)
        {
            if (menu.activeInHierarchy)
                return true;
        }
        return false;
    }

    //functions that affect hp when a msg from another game object is sent to the player
    public void TakeDamage(float dmg)
    {
        HP health = GetComponent<HP>();
        if (health != null)
        {
            health.ReduceHP(dmg);
        }
    }

    public void AddHealth(float hpBoost)
    {
        HP health = GetComponent<HP>();
        if (health != null)
        {
            health.AddHP(hpBoost);
        }
    }
}
