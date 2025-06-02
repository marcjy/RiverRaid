using UnityEngine;

[RequireComponent(typeof(ScrollVertically))]
public class MoveSideToSideEnemyBehaviour : BaseEnemyBehaviour
{
    public float Speed;
    public Sprite[] PossibleSprites;

    private LevelManager _levelManager;
    private RiverManager _riverManager;
    private SpriteRenderer _spriteRenderer;
    private ScrollVertically _scrollVertically;

    private int _minPositionX;
    private int _maxPositionX;

    private float _targetX;
    private float _lastYUsedForXBounds;
    private bool _movingLeft;

#if DEBUG
    private LevelInfo _levelInfo;
    private float _virtuaPositionY;
    private float _initialVirtuaPositionY;
#endif

    protected override void Awake()
    {
        base.Awake();

        _levelManager = FindAnyObjectByType<LevelManager>();
        _lastYUsedForXBounds = _spawnPositionY;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];

        _scrollVertically = GetComponent<ScrollVertically>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Move();

        if (HasYChangedSinceLastBoundsCheck())
        {
            CalculateXBoundsAtY(Mathf.RoundToInt(transform.position.y));
            _lastYUsedForXBounds = transform.position.y;
        }
    }

    public override void Init()
    {
        _riverManager = _levelManager.CurrentLevel.GetComponentInChildren<RiverManager>();
        CalculateXBoundsAtY(_spawnPositionY);

#if DEBUG
        _levelInfo = _levelManager.CurrentLevel.GetComponent<LevelInfo>();
        _initialVirtuaPositionY = _levelInfo.TrackeablePositionY + (_spawnPositionY * 2);
#endif

        transform.position = new Vector3(Random.Range(_minPositionX, _maxPositionX + 1), _spawnPositionY, 0);

        _movingLeft = Random.Range(0, 2) == 0;
        _targetX = _movingLeft ? _minPositionX : _maxPositionX;
        transform.rotation = _movingLeft ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);

        _scrollVertically.RecalculateSpeed();
    }

    private void Move()
    {
        Vector3 currentPosition = transform.position;

        transform.position = Vector3.MoveTowards(currentPosition, new Vector3(_targetX, currentPosition.y, currentPosition.z), Speed * Time.deltaTime);

#if DEBUG
        _virtuaPositionY = _initialVirtuaPositionY + transform.position.y;
#endif

        if (Mathf.Approximately(transform.position.x, _targetX))
        {
            _movingLeft = !_movingLeft;
            FlipEnemy();
            _targetX = _movingLeft ? _minPositionX : _maxPositionX;
        }
    }

    private void CalculateXBoundsAtY(int y)
    {
        if (!_riverManager.GetXBoundsGivenY(y + _spawnPositionY, out _minPositionX, out _maxPositionX, Mathf.FloorToInt(transform.position.x)))
            TriggerOnShouldBeReleased();

        _targetX = _movingLeft ? _minPositionX : _maxPositionX;
    }
    private bool HasYChangedSinceLastBoundsCheck()
    {
        int previousY = Mathf.RoundToInt(_lastYUsedForXBounds);
        int currentY = Mathf.RoundToInt(transform.position.y);

        return previousY != currentY;
    }

    private void FlipEnemy() => transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z * -1);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_minPositionX, transform.position.y, transform.position.z), new Vector3(_maxPositionX, transform.position.y, transform.position.z));
    }
}
