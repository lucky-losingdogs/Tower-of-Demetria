using UnityEngine;

[CreateAssetMenu]

public class EquippableSeedSO : EquippableItemSO, IDestroyableItem, IItemAction
{
    public new string m_actionName => "Use";

    [field: SerializeField] public CropID m_cropID;
}

public enum CropID
{
    Turnip, Tomato, Melon, Grape, Strawberry
}
