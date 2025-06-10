using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnStartDeath;
    public event EventHandler OnFinishDeath;

    [Header("Movement")]
    public float MoveSpeed = 2.0f;
    public float TiltSpeed = 2.0f;
    public float MaxTilt = 30.0f;
    public float TiltAnimationDuration = 1.0f;
    private float _moveDirection = 0.0f;
    private Coroutine _tiltAnimationCoroutine = null;

    [Header("Acceleration")]
    public float NormalSpeedPositionY;
    public float SlowSpeedPositionY;
    public float AccelerationAnimationDuration = 0.5f;
    public LineRenderer LineTrail;
    private Coroutine _accelerationAnimationCoroutine = null;

    [Header("Death")]
    public float DeathAnimationDuration;
    public int RotationIncrements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Player movement
        InputManager.Instance.OnPlayerMoves += HandlePlayerMoves;
        InputManager.Instance.OnPlayerStopsMoving += HandlePlayerStopsMoving;

        //Level Scroll speed
        InputManager.Instance.OnPlayerAccelerating += HandlePlayerAccelerating;

        GetComponent<PlayerCollisionDetector>().OnHazardCollision += HandleDeath;
        GetComponent<PlayerFuelManager>().OnOutOfFuel += HandleDeath;
    }

    void Update()
    {
        MoveHorizontally();
    }

    #region Event Handling
    private void HandlePlayerMoves(object sender, float direction)
    {
        _moveDirection = direction;
        BeginTilt();
    }
    private void HandlePlayerStopsMoving(object sender, System.EventArgs e)
    {
        _moveDirection = 0.0f;
        RevertTilt();
    }

    private void HandlePlayerAccelerating(object sender, float acceleration)
    {
        if (acceleration == 0)
        {
            MoveToNormalSpeedPositionY();
            LineTrail.enabled = false;
        }
        else
            if (acceleration < 0)
            MoveToSlowSpeedPositionY();
        else
            ActivateFastSpeedMode();
    }

    private void HandleDeath(object sender, EventArgs e) => StartCoroutine(PlayAnimationDeath());
    #endregion

    private void MoveHorizontally() => transform.position += new Vector3(_moveDirection, 0.0f, 0.0f) * MoveSpeed * Time.deltaTime;

    #region Tilt
    private IEnumerator TiltJet(float targetDegrees, float animationDuration)
    {
        float elapsedTime = 0.0f;
        float degrees = 0.0f;
        float startingDegrees = transform.rotation.eulerAngles.y;

        while (elapsedTime < animationDuration)
        {
            degrees = Mathf.LerpAngle(startingDegrees, targetDegrees, elapsedTime / animationDuration);
            transform.rotation = Quaternion.Euler(0.0f, degrees, 0.0f);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(0.0f, targetDegrees, 0.0f);
    }
    private void BeginTilt()
    {
        if (_tiltAnimationCoroutine != null)
            StopCoroutine(_tiltAnimationCoroutine);

        _tiltAnimationCoroutine = StartCoroutine(TiltJet(MaxTilt * _moveDirection, TiltAnimationDuration));
    }
    private void RevertTilt()
    {
        if (_tiltAnimationCoroutine != null)
            StopCoroutine(_tiltAnimationCoroutine);

        _tiltAnimationCoroutine = StartCoroutine(TiltJet(0.0f, TiltAnimationDuration));
    }
    #endregion

    #region Speed
    private IEnumerator MoveInAxisY(float targetY, float animationDuration)
    {
        float elapsedTime = 0.0f;
        float initialPositionY = transform.position.y;
        float newY = 0.0f;

        while (elapsedTime < animationDuration)
        {
            newY = Mathf.Lerp(initialPositionY, targetY, elapsedTime / animationDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    private void MoveToSlowSpeedPositionY()
    {
        if (_accelerationAnimationCoroutine != null)
            StopCoroutine(_accelerationAnimationCoroutine);

        _accelerationAnimationCoroutine = StartCoroutine(MoveInAxisY(SlowSpeedPositionY, AccelerationAnimationDuration));
    }
    private void MoveToNormalSpeedPositionY()
    {
        if (_accelerationAnimationCoroutine != null)
            StopCoroutine(_accelerationAnimationCoroutine);

        _accelerationAnimationCoroutine = StartCoroutine(MoveInAxisY(NormalSpeedPositionY, AccelerationAnimationDuration));
    }
    private void ActivateFastSpeedMode()
    {
        LineTrail.enabled = true;
    }
    #endregion

    private IEnumerator PlayAnimationDeath()
    {
        OnStartDeath?.Invoke(this, EventArgs.Empty);

        float elapsedTime = 0.0f;
        Vector3 initRotation = transform.rotation.eulerAngles;
        Vector3 initScale = transform.localScale;

        Vector3 newRotation = initRotation;

        while (elapsedTime < DeathAnimationDuration)
        {
            newRotation.z += RotationIncrements;

            transform.rotation = Quaternion.Euler(newRotation);
            transform.localScale = Vector3.Lerp(initScale, new Vector3(0.0f, 0.0f, 0.0f), elapsedTime / DeathAnimationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(initRotation);
        transform.localScale = initScale;

        OnFinishDeath?.Invoke(this, EventArgs.Empty);
    }
}
