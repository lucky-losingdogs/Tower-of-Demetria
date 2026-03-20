using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
{
    //a list of the modifiers an item can perform
    [SerializeField] private List<ModifierData> m_modifiersDataList = new List<ModifierData>();

    //returns "Consume"
    public string m_actionName => "Consume";

    //can be set in the inspector
    [field: SerializeField] public AudioClip m_actionSFX { get; private set; }


    public bool PerformAction(GameObject character, int itemQuantity = 0)
    {
        foreach (ModifierData data in m_modifiersDataList)
        {
            data.m_statModifier.AffectCharacter(character, data.m_value);
        }
        return true;
    }
}

[Serializable]
public class ModifierData
{
    public CharacterStatModifierSO m_statModifier;
    public float m_value;
}
