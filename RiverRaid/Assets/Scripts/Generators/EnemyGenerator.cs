using System;

public class EnemyGenerator : ObjectGenerator<BaseEnemyBehaviour>
{
    public static event EventHandler<BaseEnemyBehaviour> OnEnemySpawn;
    public static event EventHandler<BaseEnemyBehaviour> OnEnemyReleased;

    protected override void Awake()
    {
        base.Awake();
        base.OnObjectGet += HandleObjectGot;
        base.OnObjectReleased += HandleObjectReleased;
    }

    private void HandleObjectGot(object sender, BaseEnemyBehaviour enemySpawned) => OnEnemySpawn?.Invoke(this, enemySpawned);
    private void HandleObjectReleased(object sender, BaseEnemyBehaviour enemyReleased) => OnEnemyReleased?.Invoke(this, enemyReleased);
}
