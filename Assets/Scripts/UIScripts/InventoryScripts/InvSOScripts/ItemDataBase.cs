using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    [SerializeField] private ItemSO[] m_itemSOArr;

    public ItemSO MatchItemID(ItemID itemID)
    {
        for (int i = 0; i < m_itemSOArr.Length; i++)
        {
            if (m_itemSOArr[i].m_itemID == itemID)
            {
                return m_itemSOArr[i];
            }
        }
        return null;
    }
}
