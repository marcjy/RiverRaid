using System.Collections;
using UnityEngine;

public class TrackPlayerEnemyBehaviour : BaseEnemyBehaviour
{
    public float Speed;

    public float RotationOffsetZ;
    public int MinSpawnPositionX;
    public int MaxSpawnPositionX;

    [Tooltip("Quantity of seconds that have to pass to recalculate the path from the enemy to the player.")]
    public float RepathInterval;

    private Coroutine _trackPlayerCoroutine;



    protected override void Update()
    {
        //Cannot use the base's Update because we need to stop the coroutine before releasing the enemy from the ObjectPool.
        //base.Update();

        if (transform.position.y <= _minPositionY)
            ReleaseEnemy();
    }

    #region Event Handling
    private void HandleSpeedChanged(object sender, float newSpeed) => Speed = newSpeed;
    #endregion

    public override void Init()
    {
        transform.position = new Vector3(Random.Range(MinSpawnPositionX, MaxSpawnPositionX + 1), _spawnPositionY, 0);
        _trackPlayerCoroutine = StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        Vector3 playerPosition = GameManager.Instance.Player.transform.position;
        LookAt(playerPosition);
        float elapsedTime = 0.0f;

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, Speed * Time.deltaTime);

            if (elapsedTime >= RepathInterval)
            {
                LookAt(playerPosition);
                playerPosition = GameManager.Instance.Player.transform.position;
                elapsedTime = 0.0f;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private void ReleaseEnemy()
    {
        StopCoroutine(_trackPlayerCoroutine);
        _trackPlayerCoroutine = null;

        base.TriggerOnShouldBeReleased();
    }
    private void LookAt(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + RotationOffsetZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            ReleaseEnemy();
    }
}
