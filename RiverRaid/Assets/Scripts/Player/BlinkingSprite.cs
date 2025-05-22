using System.Collections;
using UnityEngine;

public class BlinkingSprite : MonoBehaviour
{
    public float BlinkDuration;
    public float TimeBetweenStates;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        GameManager.Instance.OnLevelEnds += HandleLevelEnds;
    }

    private void HandleLevelEnds(object sender, System.EventArgs e) => StartCoroutine(StartBlinking());

    private IEnumerator StartBlinking()
    {
        float elapsedTime = 0.0f;
        bool isVisible = true;

        while (elapsedTime < BlinkDuration)
        {
            _renderer.enabled = isVisible;
            isVisible = !isVisible;

            yield return new WaitForSeconds(TimeBetweenStates);

            elapsedTime += TimeBetweenStates;
        }

        _renderer.enabled = true;
    }
}
