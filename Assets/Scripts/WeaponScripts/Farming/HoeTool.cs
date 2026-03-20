using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeTool : WeaponBase
{
    private Tilemap m_farmableTileMap;
    private Tilemap m_ploughTileMap;
    private TileBase m_ploughTile;
    private TileBase m_farmableTile;


    public void InitialiseHoe(Tilemap farmableTileMap, Tilemap ploughTileMap, TileBase newTile, TileBase farmableTile)
    {
        m_farmableTileMap = farmableTileMap;
        m_ploughTileMap = ploughTileMap;
        m_ploughTile = newTile;
        m_farmableTile = farmableTile;
    }

    public override void Attack()
     {
        if(m_farmableTileMap == null)
        {
            Debug.LogError("m_controlTile is missing");
        }
        else if (m_ploughTile == null)
        {
            Debug.LogError("m_ploughTile is missing");
        }

            Plough();
     }

    public void Plough()
    {
        if (owner == null)
        {
            Debug.LogError("owner is missing?");
            return;
        }

        Vector2 m_faceDirection = owner.m_lastDirection;
        if (m_faceDirection == Vector2.zero)
        {
            m_faceDirection = Vector2.down;
        }

        //player world position
        Vector3Int m_playerPos = m_farmableTileMap.WorldToCell(owner.transform.position);

        //target is the world position of the tile in front of the player
        //converts the direction the player is facing from floats to rounded numbers that the tilemap can use
        Vector3Int m_targetWorldPos = m_playerPos + new Vector3Int(Mathf.RoundToInt(m_faceDirection.x), Mathf.RoundToInt(m_faceDirection.y), 0);
        Debug.Log(m_targetWorldPos);

        //if the target tile is a farmable tile
        m_farmableTile = m_farmableTileMap.GetTile(m_targetWorldPos);
        Debug.Log(m_farmableTile);

        if (m_farmableTile != null)
        {
            //convert world position to tile cell
            m_ploughTileMap.SetTile(m_targetWorldPos, m_ploughTile);
        }
    }   
}
