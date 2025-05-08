using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject InitialLevel;
    public GameObject[] LevelPrefabs;
    private List<int> _unusedLevelIndexes = new List<int>();

    [Header("Scroll Speed")]
    public float NormalSpeed = 2.0f;
    public float SlowSpeed = 1.0f;
    public float FastSpeed = 4.0f;
    private float _currentSpeed;

    private GameObject _currentLevel;
    private GameObject _nextLevel;

    private void Awake()
    {
        _currentSpeed = NormalSpeed;
        enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerAccelerating += HandlePlayerAccelerating;

        GameManager.Instance.OnStartNewGame += HandleStartNewGame;
        GameManager.Instance.OnEndGame += HandleEndGame;
        GameManager.Instance.OnResetGame += HandleResetGame;
    }



    #region EventHandling
    private void HandleStartNewGame(object sender, System.EventArgs e)
    {
        InstantiateInitialLevels();

        _currentSpeed = NormalSpeed;
        enabled = true;
    }
    private void HandleEndGame(object sender, System.EventArgs e)
    {
        enabled = false;
    }
    private void HandleResetGame(object sender, System.EventArgs e)
    {
        Destroy(_currentLevel);
        Destroy(_nextLevel);

        _currentLevel = null;
        _nextLevel = null;

        _unusedLevelIndexes.Clear();
    }

    private void HandlePlayerAccelerating(object sender, float acceleration)
    {
        if (acceleration == 0)
            _currentSpeed = NormalSpeed;
        else
            if (acceleration < 0)
            _currentSpeed = SlowSpeed;
        else
            _currentSpeed = FastSpeed;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        ScrollLevels();
    }

    private void InstantiateInitialLevels()
    {
        _currentLevel = Instantiate(InitialLevel, transform);
        _nextLevel = Instantiate(GetRandomUnusedLevel(), transform);

        float nextLevelOffsetY = _currentLevel.GetComponent<LevelInfo>().BridgePositionY + 0.9f; //Overlap a bit the two TileMaps to avoid empy spaces between the current and next level.
        _nextLevel.transform.position = new Vector3(0.0f, nextLevelOffsetY, 0.0f);
    }

    private void InstantiateNewLevel()
    {
        Destroy(_currentLevel);

        _currentLevel = _nextLevel;
        _nextLevel = Instantiate(GetRandomUnusedLevel(), transform);

        float nextLevelOffsetY = _currentLevel.GetComponent<LevelInfo>().BridgePositionY + 0.9f;
        _nextLevel.transform.position = new Vector3(0.0f, nextLevelOffsetY, 0.0f);
    }

    private void ScrollLevels()
    {
        _currentLevel.transform.position -= Vector3.up * _currentSpeed * Time.deltaTime;
        _nextLevel.transform.position -= Vector3.up * _currentSpeed * Time.deltaTime;

        if (_nextLevel.transform.position.y < 0)
            InstantiateNewLevel();
    }

    private GameObject GetRandomUnusedLevel()
    {
        int nLevels = LevelPrefabs.Length;

        if (_unusedLevelIndexes.Count == 0)
        {
            for (int i = 0; i < nLevels; i++)
                _unusedLevelIndexes.Add(i);
        }

        int rndIndex = Random.Range(0, nLevels);
        _unusedLevelIndexes.Remove(rndIndex);

        return LevelPrefabs[rndIndex];
    }

}
