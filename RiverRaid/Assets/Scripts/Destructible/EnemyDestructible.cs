using Unity.VisualScripting;
using UnityEngine;

public class EnemyDestructible : BaseDestructible
{

    protected override void OnDestroyedInternal(GameObject source)
    {
        DestroyOnAnimationEvent destroyAnimation = Instantiate(DestroyAnimation);
        destroyAnimation.AddComponent<ScrollVertically>().RecalculateSpeed();
        destroyAnimation.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
