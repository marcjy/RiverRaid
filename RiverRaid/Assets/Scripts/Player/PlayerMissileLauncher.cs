using UnityEngine;

public class PlayerMissileLauncher : MonoBehaviour
{
    public MissileCollisionDetector MissilePrefab;
    public float MissileSpeed;
    public float MissileLauncherCoolDown;
    public float OffsetPositionY;

    private float _elapsedTimeSinceLastLaunch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _elapsedTimeSinceLastLaunch = MissileLauncherCoolDown;
        InputManager.Instance.OnPlayerFires += HandlePlayerFires;
    }

    private void Update()
    {
        _elapsedTimeSinceLastLaunch += Time.deltaTime;
    }

    #region Event Handling
    private void HandlePlayerFires(object sender, System.EventArgs e)
    {
        if (_elapsedTimeSinceLastLaunch >= MissileLauncherCoolDown)
            Fire();
    }
    #endregion

    private void Fire()
    {
        Rigidbody2D missile = Instantiate(MissilePrefab).GetComponent<Rigidbody2D>();
        missile.transform.position = new Vector3(transform.position.x, transform.position.y + OffsetPositionY);
        missile.linearVelocityY = MissileSpeed;

        _elapsedTimeSinceLastLaunch = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + OffsetPositionY), 0.1f);
    }
}
