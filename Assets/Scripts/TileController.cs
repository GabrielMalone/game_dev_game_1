using UnityEngine;
using UnityEngine.Tilemaps;



public class TileController : MonoBehaviour
{
    public TileBase tileSquare;
    public Tilemap tilemap;

    void Start()
    {
        // Turn OFF the tile at grid position (2, 3)
        tilemap.SetTile(new Vector3Int(2, 3, 0), null);
    }
}