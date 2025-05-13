using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeManager : MonoBehaviour, IDestructible
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

    // Update is called once per frame
    void Update()
    {

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
    private void SetDestructibleTiles() => _destuctibleTiles = _tilePositions.Skip(1).Take(_tilePositions.Count - 2).ToList(); //The first and last tiles shouldn't be destroyed.

    public void Destroy(GameObject source)
    {
        float offsetPositionY = 0.5f;
        float offsetPositionX = 0.5f;

        foreach (Vector3Int position in _destuctibleTiles)
        {
            DestroyOnAnimationEvent explosion = Instantiate(ExplosionAnimation);
            explosion.transform.position = new Vector3(position.x + offsetPositionX, source.transform.position.y + offsetPositionY);
            explosion.GetComponent<ScrollVertically>().RecalculateSpeed();

            _tileMap.SetTile(position, null);
        }
    }
}
