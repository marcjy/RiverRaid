using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInfo : MonoBehaviour
{
    public Tilemap BridgeTilemap;
    public float BridgePositionY { get; private set; }

    private const int OFFSET_Y = 6;

    private void Awake()
    {
        BoundsInt bounds = BridgeTilemap.cellBounds;
        BridgePositionY = bounds.yMax + OFFSET_Y;
    }
}
