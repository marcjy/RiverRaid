using UnityEngine;

public class ScrollVertically : MonoBehaviour
{
    [Header("Scroll Speed")]
    public float NormalSpeed = 2.0f;
    public float SlowSpeed = 1.0f;
    public float FastSpeed = 4.0f;
    private float _currentSpeed;

    private void Awake()
    {
        _currentSpeed = 0.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerAccelerating += HandlePlayerAccelerating;

        GameManager.Instance.OnStartLevel += HandleStartLevel;
        GameManager.Instance.OnResetGame += HandleGameStateReset;
        GameManager.Instance.OnEndGame += HandleGameStateReset;
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    #region EventHandling
    private void HandleStartLevel(object sender, System.EventArgs e) => _currentSpeed = NormalSpeed;
    private void HandleGameStateReset(object sender, System.EventArgs e) => _currentSpeed = 0.0f;

    private void HandlePlayerAccelerating(object sender, float acceleration) => CalculateSpeed(acceleration);
    #endregion

    private void CalculateSpeed(float acceleration)
    {
        if (acceleration == 0)
            _currentSpeed = NormalSpeed;
        else
            if (acceleration < 0)
            _currentSpeed = SlowSpeed;
        else
            _currentSpeed = FastSpeed;
    }
    public void RecalculateSpeed()
    {
        //If the player is accelerating/decelerating the speed won't be updated, due to the event not triggering because the player is holding down the corresponding key.
        CalculateSpeed(InputManager.Instance.Acceleration.action.ReadValue<float>());
    }

    private void Scroll() => gameObject.transform.position -= Vector3.up * _currentSpeed * Time.deltaTime;
}
