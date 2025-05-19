using UnityEngine;

public class ScrollVertically : MonoBehaviour
{
    private float _currentSpeed;

    private void Awake()
    {
        _currentSpeed = 0.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpeedManager.OnSpeedChanged += HandleSpeedChanged;
    }


    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    #region EventHandling
    private void HandleSpeedChanged(object sender, float newSpeed) => _currentSpeed = newSpeed;
    #endregion

    private void Scroll() => gameObject.transform.position -= _currentSpeed * Time.deltaTime * Vector3.up;

    public void RecalculateSpeed() => _currentSpeed = SpeedManager.CurrentSpeed;
}
