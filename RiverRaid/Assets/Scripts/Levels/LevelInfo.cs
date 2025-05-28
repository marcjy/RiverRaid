using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInfo : MonoBehaviour
{
    public Tilemap BridgeTilemap;

    [Header("Debug")]
    private TextMeshProUGUI _trackablePositionYUIValue;

    public float BridgePositionY { get; private set; }

    public float TrackeablePositionY
    {
        get
        {
            return _trackeablePositionY % BridgePositionY;
        }

        private set => _trackeablePositionY = value;
    }

    private float _trackeablePositionY;
    private const int OFFSET_Y = 6;

    private void Awake()
    {
        BoundsInt bounds = BridgeTilemap.cellBounds;
        BridgePositionY = bounds.yMax + OFFSET_Y;

        _trackablePositionYUIValue = GameObject.Find("TrackeablePositionY-Value").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance.OnStartLevel += HandleStartLevel;
    }

    private void HandleStartLevel(object sender, System.EventArgs e) => _trackeablePositionY = Camera.main.orthographicSize * -1;


    private void Update()
    {
        _trackeablePositionY += SpeedManager.CurrentSpeed * Time.deltaTime;
        _trackablePositionYUIValue.text = _trackeablePositionY.ToString("F2");
    }
}
