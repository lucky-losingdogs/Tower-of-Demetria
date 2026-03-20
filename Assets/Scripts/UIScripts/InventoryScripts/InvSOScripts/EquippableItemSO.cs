using UnityEngine;

[CreateAssetMenu]
public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
{
    //returns "Equip"
    public string m_actionName => "Equip";
    [field: SerializeField] public AudioClip m_actionSFX { get; private set; }

    [field: SerializeField] public float m_attackRate;

    [field: SerializeField] public WeaponType m_weaponType;


    public bool PerformAction(GameObject character, int itemQuantity = 1)
    {
        //gets the EquipWeapon script on the player
        EquipWeapon weaponSystem = character.GetComponent<EquipWeapon>();
        if (weaponSystem != null)
        {
            //sets the player's weapon to this weapon so
            weaponSystem.SetWeapon(this, itemQuantity);
            return true;
        }
        return false;
    }
}

public enum WeaponType
{
    Ranged, Melee, Hoe, Seed, Empty
}