using UnityEngine;

public class EnemyDestructible : BaseDestructible
{

    protected override void OnDestroyedInternal(GameObject source)
    {
        DestroyOnAnimationEvent destroyAnimation = Instantiate(DestroyAnimation);
        destroyAnimation.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
