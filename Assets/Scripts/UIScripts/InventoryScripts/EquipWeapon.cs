using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    //the player's current weapon
    [SerializeField]
    public EquippableItemSO m_currentWeapon;

    //reference to the inventory data
    [SerializeField]
    private InventorySO inventoryData;

    //reference to the data weapons need to use
    [SerializeField] WeaponData m_weaponData;

    //reference to player
    private TopDownCharacterController m_topDownCharacterController;

    //reference to weapon parent
    public WeaponBase m_currentWeaponComponent;

    private void Awake()
    {
        m_topDownCharacterController = GetComponent<TopDownCharacterController>();
    }

    public void SetWeapon(EquippableItemSO weaponItemSO, int itemQuantity)
    {
        //adds the weapon back to the inventory if it's unequipped
        if (m_currentWeapon != null)
        {
            inventoryData.AddItem(m_currentWeapon, 1);
        }

        //sets current weapon to the equipped weaponItemSO from the inv
        m_currentWeapon = weaponItemSO;
        InitialiseCurrentWeapon(itemQuantity);
    }

    public void UnequipWeapon()
    {
        m_currentWeapon = null;
    }

    public void InitialiseCurrentWeapon(int itemQuantity = 1)
    {
        //Remove the old weapon component
        if (m_currentWeaponComponent != null)
        {
            Destroy(m_currentWeaponComponent);
            m_currentWeaponComponent = null;
        }

        //add the new weapon script component, and initialise it to pass the necessary variables it uses
        switch (m_currentWeapon.m_weaponType)
        {
            case WeaponType.Hoe:
                HoeTool hoe = gameObject.AddComponent<HoeTool>();
                hoe.InitialiseWeaponBase(m_topDownCharacterController);
                
                if (hoe != null)
                    hoe.InitialiseHoe(m_weaponData.m_farmableTileMap, m_weaponData.m_ploughTileMap, m_weaponData.m_ploughTile, m_weaponData.m_farmableTile);
                m_currentWeaponComponent = hoe;
                break;

            case WeaponType.Melee:
                MeleeWeapon melee = gameObject.AddComponent<MeleeWeapon>();
                melee.InitialiseWeaponBase(m_topDownCharacterController);
                m_currentWeaponComponent = melee;
                break;

            case WeaponType.Ranged:
                RangedWeapon ranged = gameObject.AddComponent<RangedWeapon>();
                ranged.InitialiseWeaponBase(m_topDownCharacterController);

                if (ranged != null)
                    ranged.InitialiseRanged(m_weaponData.m_projectilePrefab, m_weaponData.m_projectileSpeed);
                m_currentWeaponComponent = ranged;
                break;

            case WeaponType.Seed:
                PlantSeed seed = gameObject.AddComponent<PlantSeed>();
                seed.InitialiseWeaponBase(m_topDownCharacterController);

                if (seed != null)
                {
                    //takes the current seed as the seedSO and passes the correct seedSO to PlantSeed
                    EquippableSeedSO seedSO = m_currentWeapon as EquippableSeedSO;
                    if (seedSO == null)
                        return;

                    seed.InitialiseSeed(m_weaponData.m_cropLib, m_weaponData.m_ploughTileMap, m_weaponData.m_cropTileMap, m_weaponData.m_ploughTile, seedSO, itemQuantity, this, m_weaponData.m_cropGrowing);
                }
                m_currentWeaponComponent = seed;
                break;

            case WeaponType.Empty:
                m_currentWeaponComponent = null;
                break;
        }
    }
}