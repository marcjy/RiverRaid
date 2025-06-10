using System;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    public event EventHandler OnHazardCollision;

    private readonly float _ignoreCollisionsDuration = 2.0f;
    private float _ignoreCollisionsCounter = 0.0f;
    private bool _isIgnoringCollisions = false;

    private void Update()
    {
        if (_isIgnoringCollisions)
            _ignoreCollisionsCounter += Time.deltaTime;

        if (_ignoreCollisionsCounter >= _ignoreCollisionsDuration)
        {
            _ignoreCollisionsCounter = 0.0f;
            _isIgnoringCollisions = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isIgnoringCollisions)
            return;

        if (collision.gameObject.CompareTag("Hazard"))
        {
            OnHazardCollision?.Invoke(this, EventArgs.Empty);
            _isIgnoringCollisions = true;
        }

        if (collision.TryGetComponent(out ICollectable collectable))
            collectable.Collect(gameObject);
    }
}