using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseDestructible : MonoBehaviour
{
    public event EventHandler<int> OnDestroyed;

    public int ScoreValue;

    public DestroyOnAnimationEvent DestroyAnimation;
    [SerializeField] private AudioClip _destroySound;

    public void Destroy(GameObject source)
    {
        OnDestroyed?.Invoke(this, ScoreValue);

        AudioManager.PlaySFX(_destroySound);

        OnDestroyedInternal(source);
    }

    protected virtual void OnDestroyedInternal(GameObject source)
    {
        DestroyOnAnimationEvent destroyAnimation = Instantiate(DestroyAnimation);
        destroyAnimation.AddComponent<ScrollVertically>().RecalculateSpeed();
        destroyAnimation.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
