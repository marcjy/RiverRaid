using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInfo : MonoBehaviour
{
    public Tilemap BridgeTilemap;
    public float BridgePositionY { get; private set; }

    private void Awake()
    {
        BoundsInt bounds = BridgeTilemap.cellBounds;
        BridgePositionY = bounds.yMax;
    }
}
