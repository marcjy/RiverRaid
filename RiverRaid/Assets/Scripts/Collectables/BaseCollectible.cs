using System;
using UnityEngine;

public abstract class BaseCollectible : MonoBehaviour, ICollectable, IGenerable
{
    public event EventHandler OnShouldBeReleased;
    public event EventHandler<int> OnCollected;

    public int ScoreValue;
    public AudioClip CollectedSound;

    protected int _spawnPositionY;
    protected int _minSpawnPositionX;
    protected int _maxSpawnPositionX;

    private float _minPositionY;

    private void Awake()
    {
        _spawnPositionY = Mathf.CeilToInt(Camera.main.transform.position.y + Camera.main.orthographicSize);
        _minPositionY = GameManager.Instance.Player.transform.position.y - 1;
    }

    private void Start()
    {
        RiverManager riverManager = FindAnyObjectByType<LevelManager>().CurrentLevel.GetComponentInChildren<RiverManager>();
        riverManager.GetXBoundsGivenY(_spawnPositionY, out _minSpawnPositionX, out _maxSpawnPositionX);

        GetComponent<ScrollVertically>().RecalculateSpeed();
        GetComponent<CollectableDestructible>().OnDestroyed += HandleDestroyed;
    }

    protected virtual void Update()
    {
        if (transform.position.y <= _minPositionY)
            TriggerShouldBeReleased();
    }

    #region Event Handling
    private void HandleDestroyed(object sender, int e) => TriggerShouldBeReleased();
    #endregion

    public virtual void Init() => transform.position = new Vector3(UnityEngine.Random.Range(_minSpawnPositionX, _maxSpawnPositionX + 1), _spawnPositionY, 0);
    public virtual void Collect(GameObject player)
    {
        OnCollected?.Invoke(this, ScoreValue);

        AudioManager.PlaySFX(CollectedSound);

        CollectInternal(player);
    }
    public abstract void CollectInternal(GameObject player);

    protected void TriggerShouldBeReleased() => OnShouldBeReleased?.Invoke(this, EventArgs.Empty);

}
