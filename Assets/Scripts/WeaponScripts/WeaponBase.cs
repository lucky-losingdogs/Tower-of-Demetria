using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public float m_attackRate = 1;

    //attack function used by all weapons
    public abstract void Attack();

    //reference to the player character
    public TopDownCharacterController owner;


    //store data from the EquippableItemSO in the weapon parent
    //that other weapons inherit from
    public void InitialiseWeaponBase(TopDownCharacterController player)
    {
        owner = player;
    }
}
