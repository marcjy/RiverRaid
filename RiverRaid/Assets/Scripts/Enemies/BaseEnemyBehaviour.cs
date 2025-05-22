using System;
using UnityEngine;

[RequireComponent(typeof(EnemyDestructible))]
public abstract class BaseEnemyBehaviour : MonoBehaviour, IGenerable
{
    public event EventHandler OnShouldBeReleased;

    protected float _minPositionY;
    protected int _spawnPositionY;

    protected EnemyDestructible _enemyDestructible;

    protected virtual void Awake()
    {
        _enemyDestructible = GetComponent<EnemyDestructible>();
        _enemyDestructible.OnDestroyed += HandleDestroyed;

        _spawnPositionY = Mathf.CeilToInt(Camera.main.transform.position.y + Camera.main.orthographicSize);
        _minPositionY = FindAnyObjectByType<PlayerController>().gameObject.transform.position.y - 1;
    }

    protected virtual void Update()
    {
        if (transform.position.y <= _minPositionY)
            TriggerOnShouldBeReleased();
    }

    public abstract void Init();

    protected void TriggerOnShouldBeReleased() => OnShouldBeReleased?.Invoke(this, EventArgs.Empty);

    #region Event Handling
    private void HandleDestroyed(object sender, int e) => TriggerOnShouldBeReleased();
    #endregion

}
