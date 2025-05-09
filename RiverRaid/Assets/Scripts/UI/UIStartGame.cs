using System.Collections;
using UnityEngine;

public class UIStartGame : MonoBehaviour
{
    public RectTransform TextGroup;
    public float ScrollUpAnimationDuration;

    private Vector2 _textGroupInitalPosition;

    private void Awake()
    {
        _textGroupInitalPosition = TextGroup.anchoredPosition;
    }

    private void Start()
    {
        InputManager.Instance.OnStartPressed += (sender, e) => StartCoroutine(ScrollUpAnimation());

        GameManager.Instance.OnResetGame += HandleResetGame;
    }

    #region Event Handling
    private void HandleResetGame(object sender, System.EventArgs e) => TextGroup.anchoredPosition = _textGroupInitalPosition;
    #endregion


    private IEnumerator ScrollUpAnimation()
    {
        float elapsedTime = 0.0f;
        float initialPositionY = _textGroupInitalPosition.y;
        float targetPositionY = TextGroup.rect.height;
        float newY = 0.0f;

        while (elapsedTime < ScrollUpAnimationDuration)
        {
            newY = Mathf.Lerp(initialPositionY, targetPositionY, elapsedTime / ScrollUpAnimationDuration);
            TextGroup.anchoredPosition = new Vector2(_textGroupInitalPosition.x, newY);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        TextGroup.anchoredPosition = new Vector2(_textGroupInitalPosition.x, targetPositionY);

        UIEvents.TriggerIntroAnimaitonCompleted();
    }
}
