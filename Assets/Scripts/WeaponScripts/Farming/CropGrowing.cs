using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public delegate void PlantPlantedHandler(CropID ID);
public class CropGrowing : MonoBehaviour
{
    //event that handles the plant growth
    public event PlantPlantedHandler OnGrow;

    [SerializeField] private CropLibrary m_cropLibrary;
    [SerializeField] private Tilemap m_ploughedTileMap;

    public IEnumerator StartGrowing(int timeToGrow, int stages, CropID ID)
    {
        for (int i = 0; i < stages; i++)
        {
            yield return new WaitForSeconds(timeToGrow);
            //event which changes the tile of the plant
            OnGrow?.Invoke(ID);
        }
    }

    public void CropGrown(CropTile grownCrop)
    {
        //if the crop is grown, spawn the crop prefab that attacks enemies
        if (grownCrop.m_isGrown)
        {
            m_cropLibrary.m_spawnCropPrefab.SpawnPrefab(grownCrop.cropID, grownCrop);
            Debug.Log("crop prefab cloned");
        }

        //unsubscribe any functions from events to prevent this continuing to happen
        OnGrow -= grownCrop.HandleGrow;
        grownCrop.OnFullyGrown -= CropGrown;

        //change the tile back to normal a normal tile
        m_ploughedTileMap.SetTile(grownCrop.cropLocation, null);
        grownCrop.cropTileMap.SetTile(grownCrop.cropLocation, null);
    }

    public CropTile CloneCropTile(CropID m_plantID)
    {
        //checks if the plantID is in the dictionary and returns it in CropTile
        CropTile m_cropTile = m_cropLibrary.GetCropTemplate(m_plantID);
        if (m_cropTile == null)
            return m_cropTile;

        //creates a clone of CropTile so everything happens to the
        //clone and not the CropTile itself
        CropTile m_cropTileClone = (CropTile)m_cropTile.Clone();

        //subscribe HandleGrow to OnGrow event
        OnGrow += m_cropTileClone.HandleGrow;

        //subscribe CropGrown to OnFullyGrown
        m_cropTileClone.OnFullyGrown += CropGrown;

        return m_cropTileClone;
    }
}

public struct GrowthStage
{
    public TileBase m_tile;
}