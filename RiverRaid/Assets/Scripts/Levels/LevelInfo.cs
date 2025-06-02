using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInfo : MonoBehaviour
{
    public Tilemap BridgeTilemap;

    [Header("Debug")]
    private TextMeshProUGUI _trackablePositionYUIValue;

    public float BridgePositionY { get; private set; }

    public float TrackeablePositionY => _trackeablePositionY;
    private float _trackeablePositionY;
    private const int OFFSET_Y = 6;

    private void Awake()
    {
        BoundsInt bounds = BridgeTilemap.cellBounds;
        BridgePositionY = bounds.yMax + OFFSET_Y;

        _trackeablePositionY = Camera.main.orthographicSize * -1;

#if DEBUG
        _trackablePositionYUIValue = GameObject.Find("TrackeablePositionY-Value").GetComponent<TextMeshProUGUI>();
        GameObject.Find("TrackeablePositionY-Title").GetComponent<TextMeshProUGUI>().text = "TrackeablePositionY:";
#endif
    }

    private void Start()
    {
        GameManager.Instance.OnStartLevel += HandleResetTrackeablePositionY;
        LevelManager.OnReachedNewLevel += HandleResetTrackeablePositionY;
    }

    private void HandleResetTrackeablePositionY(object sender, System.EventArgs e) => _trackeablePositionY = Camera.main.orthographicSize * -1;


    private void Update()
    {
        _trackeablePositionY += SpeedManager.CurrentSpeed * Time.deltaTime;

#if DEBUG
        _trackablePositionYUIValue.text = _trackeablePositionY.ToString("F2");
#endif
    }
}
