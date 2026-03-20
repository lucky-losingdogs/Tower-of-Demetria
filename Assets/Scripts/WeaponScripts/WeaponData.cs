using UnityEngine;
using UnityEngine.Tilemaps;

public class WeaponData : MonoBehaviour
{
    [Header ("Ranged Weapon Variables")]
    public GameObject m_projectilePrefab;
    public float m_projectileSpeed;


    [Header("Melee Weapon Variables")]
    //melee vars


    [Header("Hoe Tool Variables")]
    public Tilemap m_farmableTileMap;
    public Tilemap m_ploughTileMap;
    public RuleTile m_ploughTile;
    public TileBase m_farmableTile;


    [Header("Seed Variables")]
    public CropLibrary m_cropLib;
    public Tilemap m_cropTileMap;
    public CropGrowing m_cropGrowing;
}