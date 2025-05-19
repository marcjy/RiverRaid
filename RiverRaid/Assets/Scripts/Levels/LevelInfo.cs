using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInfo : MonoBehaviour
{
    public Tilemap BridgeTilemap;
    public float BridgePositionY { get; private set; }
    public float TrackeablePositionY
    {
        get
        {
            return _trackeablePositionY % BridgePositionY;
        }

        private set => _trackeablePositionY = value;
    }

    private ScrollVertically _scrollVertically;
    private float _trackeablePositionY;
    private const int OFFSET_Y = 6;

    private void Awake()
    {
        BoundsInt bounds = BridgeTilemap.cellBounds;
        BridgePositionY = bounds.yMax + OFFSET_Y;

        _trackeablePositionY = 0;
        _scrollVertically = GetComponent<ScrollVertically>();
    }

    private void Update()
    {
        _trackeablePositionY += _scrollVertically.CurrentSpeed * Time.deltaTime;
    }
}
