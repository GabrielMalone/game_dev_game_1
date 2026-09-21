using UnityEngine;
using UnityEngine.Tilemaps;



public class TileController : MonoBehaviour
{
    public TileBase tileSquare;
    public Tilemap tilemap;

    void Start()
    {
        
    }

    void fillTileMap()
    {
        BoundsInt bounds = tilemap.cellBounds;

        int minX = bounds.xMin;
        int maxX = bounds.xMax;

        int minY = bounds.yMin;
        int maxY = bounds.yMax;

        Debug.Log("minX: " + minX);
        Debug.Log("maxX: " + maxX);
        Debug.Log("minY: " + minY);
        Debug.Log("maxY: " + maxY);


        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(position))
                {
                    tilemap.SetTileFlags(position, TileFlags.None);
                    tilemap.SetColor(position, Color.red);
                }
            }
        }
    }

}