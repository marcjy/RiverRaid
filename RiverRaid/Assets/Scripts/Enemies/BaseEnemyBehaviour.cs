using System;
using UnityEngine;

public abstract class BaseEnemyBehaviour : MonoBehaviour, IGenerable
{
    public event EventHandler OnShouldBeReleased;

    protected EnemyDestructible _enemyDestructible;

    private void Start()
    {
        _enemyDestructible = GetComponent<EnemyDestructible>();
        _enemyDestructible.OnDestroyed += HandleDestroyed;
    }

    public abstract void Init();

    #region Event Handling
    private void HandleDestroyed(object sender, int e) => OnShouldBeReleased?.Invoke(this, EventArgs.Empty);
    #endregion

}
