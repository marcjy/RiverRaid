using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RiverManager : MonoBehaviour
{
    public bool RiverIsBifurcated = false;

    [Header("Bifurcated River")]
    public int BifurcationStartsAtY;
    public int BifurcationEndsAtY;

    private int _leftRiverMinX = 0;
    private int _leftRiverMaxX = 0;
    private int _rightRiverMinX = 0;
    private int _rightRiverMaxX = 0;

    private Tilemap _tileMap;
    private List<Vector3Int> _tilePositions;

    private LevelInfo _levelInfo;

    private void Start()
    {
        _levelInfo = gameObject.transform.parent.GetComponent<LevelInfo>();

        _tileMap = GetComponent<Tilemap>();
        _tilePositions = new List<Vector3Int>();

        FindPaintedTiles();

        if (RiverIsBifurcated)
            FindBifurcatedRiverBounds();
    }

    public bool GetXBoundsGivenY(int y, out int minX, out int maxX, int currentX = 0)
    {
        y += Mathf.FloorToInt(_levelInfo.TrackeablePositionY);

        if (RiverIsBifurcated && (y >= BifurcationStartsAtY && y <= BifurcationEndsAtY))
            GetBifurcatedRiverXBounds(y, out minX, out maxX, currentX);
        else
        {
            List<Vector3Int> positionsWithY = _tilePositions.Where(position => position.y == y).ToList();

            if (positionsWithY.Count > 0)
            {
                //Add and substract because the leftmost and rightmost river tiles contain water and sand, and ships should not sail to those tiles.
                minX = positionsWithY.Min(position => position.x) + 1;
                maxX = positionsWithY.Max(position => position.x) - 1;
            }
            else
            {
                minX = maxX = 0;
                Debug.Log("Error: River has no painted tiles with the value of Y == " + y);
                return false;
            }
        }

        return true;
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

    private void FindBifurcatedRiverBounds()
    {
        var bifurcatedRiverPositions = _tilePositions
            .Where(pos => pos.y >= BifurcationStartsAtY && pos.y <= BifurcationEndsAtY)
            .GroupBy(pos => pos.y);

        bool found = false;

        foreach (var row in bifurcatedRiverPositions)
        {
            var sortedByX = row.Select(pos => pos.x).OrderBy(x => x).ToList();

            for (int i = 0; i < sortedByX.Count; i++)
            {
                int currentX = sortedByX[i];
                int nextX = sortedByX[i + 1];

                if (Mathf.Abs(currentX - nextX) > 1)
                {
                    //Gap detected because there is ground between the two rivers.
                    _leftRiverMinX = sortedByX.FirstOrDefault();
                    _leftRiverMaxX = currentX;

                    _rightRiverMinX = nextX;
                    _rightRiverMaxX = sortedByX.Last();

                    found = true;
                    break;
                }
            }

            if (found) break;
        }

        Debug.Log($"Left River X Bounds: {_leftRiverMinX} to {_leftRiverMaxX}");
        Debug.Log($"Right River X Bounds: {_rightRiverMinX} to {_rightRiverMaxX}");
    }

    private void GetBifurcatedRiverXBounds(int y, out int minX, out int maxX, int currentX)
    {
        minX = currentX >= _leftRiverMinX && currentX <= _leftRiverMaxX ? _leftRiverMinX : _rightRiverMinX;
        maxX = currentX >= _leftRiverMinX && currentX <= _leftRiverMaxX ? _leftRiverMaxX : _rightRiverMaxX;
    }
}
