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
            CalculateXBoundsAtY(Mathf.FloorToInt(transform.position.y));
            _lastYUsedForXBounds = transform.position.y;
        }
    }

    public override void Init()
    {
        _riverManager = _levelManager.CurrentLevel.GetComponentInChildren<RiverManager>();
        CalculateXBoundsAtY(_spawnPositionY);

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

        if (Mathf.Approximately(transform.position.x, _targetX))
        {
            _movingLeft = !_movingLeft;
            FlipEnemy();
            _targetX = _movingLeft ? _minPositionX : _maxPositionX;
        }
    }

    private void CalculateXBoundsAtY(int y)
    {
        if (!_riverManager.GetXBoundsGivenY(y, out _minPositionX, out _maxPositionX, Mathf.FloorToInt(transform.position.x)))
            Destroy(gameObject);

        _targetX = _movingLeft ? _minPositionX : _maxPositionX;
    }
    private bool HasYChangedSinceLastBoundsCheck()
    {
        int previousY = Mathf.FloorToInt(_lastYUsedForXBounds);
        int currentY = Mathf.FloorToInt(transform.position.y);

        return previousY != currentY;
    }

    private void FlipEnemy() => transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z * -1);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(_targetX, transform.position.y, transform.position.z));
    }
}
