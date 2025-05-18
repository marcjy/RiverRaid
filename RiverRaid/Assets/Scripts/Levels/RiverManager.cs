using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverManager : MonoBehaviour
{
    private Tilemap _tileMap;
    private List<Vector3Int> _tilePositions;

    private void Start()
    {
        _tileMap = GetComponent<Tilemap>();
        _tilePositions = new List<Vector3Int>();

        FindPaintedTiles();
    }


    public void GetXBoundsGivenY(int y, out int minX, out int maxX)
    {
        List<Vector3Int> positionsWithY = _tilePositions.Where(position => position.y == y).ToList();

        if (positionsWithY.Count > 0)
        {
            minX = positionsWithY.Min(position => position.x) + 1;
            maxX = positionsWithY.Max(position => position.x) - 1;
        }
        else
        {
            minX = maxX = 0;
            Debug.Log("Error: River has no painted tiles with the value of Y == " + y);
            return;
        }
    }

    private void FindPaintedTiles()
    {
        BoundsInt bounds = _tileMap.cellBounds;

        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j < bounds.yMax; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);

                if (_tileMap.HasTile(pos))
                    _tilePositions.Add(pos);
            }
        }
    }
}
