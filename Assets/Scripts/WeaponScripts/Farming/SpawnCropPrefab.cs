using UnityEngine;

public class SpawnCropPrefab : MonoBehaviour
{
    [SerializeField] private GameObject[] m_cropPrefabsArr;
    private Transform m_cropParent;
    private GameObject m_cropPrefab;
    private CropController m_cropController;

    public void SpawnPrefab(CropID cropID, CropTile cropTileClone)
    {
        MatchCropID(cropID);

        if (m_cropPrefab == null)
            return;

        //spawn position is the centre of the tile
        Vector3 spawnPos = cropTileClone.cropTileMap.GetCellCenterWorld(cropTileClone.cropLocation);
        m_cropParent = GameObject.Find("CropParent").transform;
        GameObject spawnedCrop = Instantiate(m_cropPrefab, spawnPos, Quaternion.identity, m_cropParent);
    }

    public GameObject MatchCropID(CropID cropID)
    {
        //iterates through the crop prefabs in the array
        for (int i = 0; i < m_cropPrefabsArr.Length; i++)
        {
            //get the crop controller component on the crop prefab
            m_cropController = m_cropPrefabsArr[i].GetComponent<CropController>();

            //if the crop id set in the crop controller matches the
            //crop id of the crop that has grown end the loop
            //if not, continue to the next index
            if (m_cropController.m_cropID == cropID)
            {
                return m_cropPrefab = m_cropPrefabsArr[i];
            }
            
        }

        return null;
    }
}
