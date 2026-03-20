using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    //field: to serialze field of a property
    [field: SerializeField] public bool m_isStackable { get; private set; }

    [field: SerializeField] public int m_maxStackSize { get; private set; } = 1;

    [field: SerializeField] public string m_itemName { get; private set; }

    [field: SerializeField] [field: TextArea] public string m_itemDescription { get; private set; }

    [field: SerializeField] public Sprite m_itemImage { get; private set; }

    [field: SerializeField] public ItemID m_itemID { get; private set; }

    //returns GetInstanceID() every time it’s accessed for item ids
    public int ID => GetInstanceID();
}

public interface IDestroyableItem
{

}

public interface IItemAction
{
    //the name of the action that can be done
    //to the item is displayed in the ui
    public string m_actionName { get; }

    //audioclip for when the action is performed on the item
    public AudioClip m_actionSFX { get; }

    //performs the action on the character
    bool PerformAction(GameObject characterm, int itemQuantity = 0);
}

public enum ItemID
{
    Bow, Sword, Hoe, Turnip, Tomato, Grape, Melon, Strawberry, TurnipSeeds, TomatoSeeds, GrapeSeeds, MelonSeeds, StrawberrySeeds, MonsterParts
}