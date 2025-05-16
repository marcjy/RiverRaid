using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeManager : MonoBehaviour
{
    public DestroyOnAnimationEvent ExplosionAnimation;

    private Tilemap _tileMap;
    private List<Vector3Int> _tilePositions;
    private List<Vector3Int> _destuctibleTiles;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tileMap = GetComponent<Tilemap>();
        _tilePositions = new List<Vector3Int>();
        _destuctibleTiles = new List<Vector3Int>();

        FindPaintedTiles();
        SetDestructibleTiles();
    }

    public IReadOnlyList<Vector3Int> GetDestructibleTiles() => _destuctibleTiles.AsReadOnly();

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
    private void SetDestructibleTiles() => _destuctibleTiles = _tilePositions.Skip(1).Take(_tilePositions.Count - 2).ToList(); //The first and last tiles shouldn't be destroyed.
}
