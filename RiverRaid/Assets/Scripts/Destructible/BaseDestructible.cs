using System;
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

    protected abstract void OnDestroyedInternal(GameObject source);
}
