using System;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    public event EventHandler OnHazardCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
            OnHazardCollision?.Invoke(this, EventArgs.Empty);

        if (collision.TryGetComponent(out ICollectable collectable))
            collectable.Collect(gameObject);
    }
}