using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 2.0f;
    public float TiltSpeed = 2.0f;
    public float MaxTilt = 30.0f;
    public float TiltAnimationDuration = 1.0f;
    private float _moveDirection = 0.0f;
    private Coroutine _tiltAnimationCoroutine = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerMoves += HandlePlayerMoves;
        InputManager.Instance.OnPlayerStopsMoving += HandlePlayerStopsMoving;
    }

    void Update()
    {
        MoveHorizontally();
    }



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
}
