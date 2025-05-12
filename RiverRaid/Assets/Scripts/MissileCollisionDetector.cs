using UnityEngine;

public class MissileCollisionDetector : MonoBehaviour
{
    public float TimeToLive = 5.0f;

    private float _timeAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timeAlive = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyWhenTimeToLiveExpires();
    }

    private void DestroyWhenTimeToLiveExpires()
    {
        _timeAlive += Time.deltaTime;

        if (_timeAlive >= TimeToLive)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDestructible entity))
        {
            entity.Destroy(gameObject);
            Destroy(gameObject);
        }
    }
}
