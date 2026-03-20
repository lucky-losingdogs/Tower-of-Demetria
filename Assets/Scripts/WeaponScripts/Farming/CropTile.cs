using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropTile : IGameTile, ICloneable
{
    public GrowthStage[] GrowthStageTiles;
    public bool m_isGrown;
    private int m_currentGrowthIndex = 0;
    public int m_growthTime;
    public event Action<CropTile> OnFullyGrown;

    public CropID cropID { get; set; }
    public Vector3Int cropLocation { get; set; }
    public TileBase cropTileBase { get; set; }
    public Tilemap cropTileMap { get; set; }

    public object Clone()
    {
        //clone CropTile
        CropTile clone = (CropTile)this.MemberwiseClone();

        //reset for new clone
        clone.m_isGrown = false;
        clone.m_currentGrowthIndex = 0;

        if (GrowthStageTiles != null)
        {
            clone.GrowthStageTiles = (GrowthStage[])GrowthStageTiles.Clone();
        }

        return clone;
    }


    public void HandleGrow(CropID plantID)
    {
        if (plantID != this.cropID)
            return;

        if (cropTileMap == null || cropTileBase == null)
            return;

        if (!m_isGrown)
        {
            GrowthStage nextStage = GrowthStageTiles[m_currentGrowthIndex];

            cropTileMap.SetTile(cropLocation, nextStage.m_tile);
            cropTileBase = nextStage.m_tile;

            m_currentGrowthIndex++;
        }

        if (m_currentGrowthIndex >= GrowthStageTiles.Length)
        {
            m_isGrown = true;
            OnFullyGrown?.Invoke(this);
        }
    }
}
