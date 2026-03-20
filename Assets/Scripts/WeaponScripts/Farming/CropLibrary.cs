using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropLibrary : MonoBehaviour
{
    public SpawnCropPrefab m_spawnCropPrefab;

    public Dictionary<CropID, CropTile> Tiles;

    //crop tilesheets
    public TileBase[] m_turnipSprites;
    public TileBase[] m_tomatoSprites;
    public TileBase[] m_melonSprites;
    public TileBase[] m_grapeSprites;
    public TileBase[] m_strawberrySprites;


    private void Awake()
    {
        Tiles = new Dictionary<CropID, CropTile>();

        InitialisePlantLibrary();
    }

    private void InitialisePlantLibrary()
    {
        if (m_turnipSprites != null && m_turnipSprites.Length > 0)
        {
            Tiles.Add(CropID.Turnip, new CropTile()
            {
                cropTileBase = GetTileFromSheet(m_turnipSprites, 0),
                GrowthStageTiles = new GrowthStage[] 
                {
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_turnipSprites, 1),
                    },
                    new GrowthStage() 
                    {
                        m_tile = GetTileFromSheet(m_turnipSprites, 2),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_turnipSprites, 3),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_turnipSprites, 4),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_turnipSprites, 5),
                    },
                },
                m_growthTime = 5,
            });
        }

        if (m_tomatoSprites != null && m_tomatoSprites.Length > 0)
        {
            Tiles.Add(CropID.Tomato, new CropTile()
            {
                cropTileBase = GetTileFromSheet(m_tomatoSprites, 0),
                GrowthStageTiles = new GrowthStage[]
                {
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_tomatoSprites, 1),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_tomatoSprites, 2),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_tomatoSprites, 3),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_tomatoSprites, 4),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_tomatoSprites, 5),
                    },
                },
                m_growthTime = 1,
            });
        }

        if (m_melonSprites != null && m_melonSprites.Length > 0)
        {
            Tiles.Add(CropID.Melon, new CropTile()
            {
                cropTileBase = GetTileFromSheet(m_melonSprites, 0),
                GrowthStageTiles = new GrowthStage[]
                {
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_melonSprites, 1),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_melonSprites, 2),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_melonSprites, 3),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_melonSprites, 4),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_melonSprites, 5),
                    },
                },
                m_growthTime = 10,
            });
        }

        if (m_grapeSprites != null && m_grapeSprites.Length > 0)
        {
            Tiles.Add(CropID.Grape, new CropTile()
            {
                cropTileBase = GetTileFromSheet(m_grapeSprites, 0),
                GrowthStageTiles = new GrowthStage[]
                {
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_grapeSprites, 1),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_grapeSprites, 2),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_grapeSprites, 3),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_grapeSprites, 4),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_grapeSprites, 5),
                    },
                },
                m_growthTime = 7,
            });
        }

        if (m_strawberrySprites != null && m_strawberrySprites.Length > 0)
        {
            Tiles.Add(CropID.Strawberry, new CropTile()
            {
                cropTileBase = GetTileFromSheet(m_strawberrySprites, 0),
                GrowthStageTiles = new GrowthStage[]
                {
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_strawberrySprites, 1),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_strawberrySprites, 2),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_strawberrySprites, 3),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_strawberrySprites, 4),
                    },
                    new GrowthStage()
                    {
                        m_tile = GetTileFromSheet(m_strawberrySprites, 5),
                    },
                },
                m_growthTime = 10,
            });
        }
    }

    private TileBase GetTileFromSheet(TileBase[] cropSprites, int index)
    {
        //gets an indexed tile from the crop tilesheet
        return cropSprites[index];
    }

    public CropTile GetCropTemplate(CropID plantID)
    {
        //if plantID is in the Tiles dictionary it puts the value in CropTile crop to return to caller
        //if not it returns null
        if (!Tiles.TryGetValue(plantID, out CropTile crop))
        {
            Debug.LogError(plantID + "not in PlantLibrary");
            return null;
        }

        return crop;
    }
}

public interface IGameTile
{
    CropID cropID { get; set; }
    Vector3Int cropLocation { get; set; }
    TileBase cropTileBase { get; set; }
    Tilemap cropTileMap { get; set; }
}