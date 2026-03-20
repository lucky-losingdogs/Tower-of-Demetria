using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlantSeed : WeaponBase
{
    //tile map that contains the ploughed tiles
    private Tilemap m_ploughedTileMap;

    //tile map that contains the plant tiles
    private Tilemap m_cropTileMap;

    //the ploughed tile
    private RuleTile m_ploughedTile;

    //reference to plant library
    private CropLibrary m_cropLibrary;

    //plant id held by SeedSO
    private CropID m_plantID;

    //reference to seedSO
    private EquippableSeedSO m_seedSO;

    private int m_seedQuantity = 1;
    private int m_seedsUsed = 0;
    private EquipWeapon m_equipWeapon;

    private CropGrowing m_cropGrowing;


    //passed variable data from WeaponData
    public void InitialiseSeed(CropLibrary plantLibrary, Tilemap ploughedTileMap, Tilemap plantTileMap, RuleTile rulePloughedTile, EquippableSeedSO seedSO, int seedQuantity, EquipWeapon equipWeapon, CropGrowing cropGrowing)
    {
        m_cropLibrary = plantLibrary;
        m_ploughedTileMap = ploughedTileMap;
        m_cropTileMap = plantTileMap;
        m_ploughedTile = rulePloughedTile;
        m_seedSO = seedSO;
        m_seedQuantity = seedQuantity;
        m_equipWeapon = equipWeapon;
        m_cropGrowing = cropGrowing;
    }

    public override void Attack()
    {
        Plant();
    }

    private void Plant()
    {
        if (owner == null)
        {
            Debug.LogError("owner is missing?");
            return;
        }

        //plant id is copied over from the seedSO
        m_plantID = m_seedSO.m_cropID;
        Debug.Log($"ID grabbed from seedSO: {m_plantID}");

        CropTile m_cropTileClone = m_cropGrowing.CloneCropTile(m_plantID);
        if (m_cropTileClone == null)
            return;

        if (!CheckSeedsUsed(m_cropTileClone))
            return;

        Vector3Int m_targetWorldPos = GetPlayerDirection();

        //if the target tile has been ploughed
        TileBase m_tempPloughedTile = m_ploughedTileMap.GetTile(m_targetWorldPos);

        //if the target tile has a crop already
        if (m_cropTileMap.GetTile(m_targetWorldPos))
        {
            m_cropGrowing.OnGrow -= m_cropTileClone.HandleGrow;
            return;
        }

        //passing variables to CropTile clone to use
        m_cropTileClone.cropTileMap = m_cropTileMap;
        m_cropTileClone.cropLocation = m_targetWorldPos;
        m_cropTileClone.cropID = m_plantID;


        //if the retrieved tile matches a ploughed tile
        //(the player is trying to plant on a ploughed tile)
        if (m_ploughedTile == m_tempPloughedTile)
        {
            //convert world position to tile cell
            m_cropTileMap.SetTile(m_targetWorldPos, m_cropTileClone.cropTileBase);

            //increase the number of seeds used every time one is successfully planted
            m_seedsUsed++;

            //start a coroutine based on the plant's growth time
            m_cropGrowing.StartCoroutine(m_cropGrowing.StartGrowing(m_cropTileClone.m_growthTime, m_cropTileClone.GrowthStageTiles.Length, m_cropTileClone.cropID));
            Debug.Log("Crop planted" + m_cropTileClone.cropTileBase);

            if (!CheckSeedsUsed(m_cropTileClone))
                return;
        }
        else
        {
            //if the tile is not a ploughed tile
            //it will not be able to call HandleGrow when OnGrow is invoked
            Debug.Log("Crop not planted");
            m_cropGrowing.OnGrow -= m_cropTileClone.HandleGrow;
        }
    }

    private Vector3Int GetPlayerDirection()
    {
        //gets the direction the player is facing
        Vector2 m_faceDirection = owner.m_lastDirection;
        if (m_faceDirection == Vector2.zero)
        {
            m_faceDirection = Vector2.down;
        }


        //player world position
        Vector3Int m_playerPos = m_ploughedTileMap.WorldToCell(owner.transform.position);

        //target is the world position of the tile in front of the player
        //converts the direction the player is facing from floats to rounded numbers that the tilemap can use
        return m_playerPos + new Vector3Int(Mathf.RoundToInt(m_faceDirection.x), Mathf.RoundToInt(m_faceDirection.y), 0);
    }

    private bool CheckSeedsUsed(CropTile cropTile)
    {
        Debug.Log($"Seed quantity: {m_seedQuantity} and Seeds uses: {m_seedsUsed}");
        //if the number of seeds used > the quantity of the seeds
        //the player can't plant any more and the seeds are unequipped
        if (m_seedsUsed >= m_seedQuantity)
        {
            m_equipWeapon.UnequipWeapon();
            return false;
        }
        return true;
    }
}