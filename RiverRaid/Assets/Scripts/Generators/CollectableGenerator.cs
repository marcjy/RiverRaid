using System;

public class CollectableGenerator : ObjectGenerator<BaseCollectible>
{
    public static event EventHandler<BaseCollectible> OnCollectibleSpawn;
    public static event EventHandler<BaseCollectible> OnCollectibleReleased;

    protected override void Awake()
    {
        base.Awake();
        base.OnObjectGet += HandleObjectGot;
        base.OnObjectReleased += HandleObjectReleased;
    }

    private void HandleObjectGot(object sender, BaseCollectible collectibleSpawned) => OnCollectibleSpawn?.Invoke(this, collectibleSpawned);
    private void HandleObjectReleased(object sender, BaseCollectible collectibleReleased) => OnCollectibleReleased?.Invoke(this, collectibleReleased);
}
