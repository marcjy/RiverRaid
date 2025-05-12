using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject InitialLevel;
    public GameObject[] LevelPrefabs;
    private readonly List<int> _unusedLevelIndexes = new List<int>();


    private GameObject _currentLevel;
    private GameObject _nextLevel;

    private void Awake()
    {

        InstantiateInitialLevels();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnResetLevel += HandleResetLevel;
        GameManager.Instance.OnResetGame += HandleResetGame;
    }




    #region EventHandling
    private void HandleResetLevel(object sender, System.EventArgs e)
    {
        _currentLevel.transform.position = new Vector3(0, 0, 0);
        PlaceNextLevelAboveCurrent();

    }
    private void HandleResetGame(object sender, System.EventArgs e)
    {
        Destroy(_currentLevel);
        Destroy(_nextLevel);

        _currentLevel = null;
        _nextLevel = null;

        _unusedLevelIndexes.Clear();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (_nextLevel.transform.position.y < 0)
            InstantiateNewLevel();
    }

    private void InstantiateInitialLevels()
    {
        _currentLevel = Instantiate(InitialLevel, transform);
        _nextLevel = Instantiate(GetRandomUnusedLevel(), transform);

        PlaceNextLevelAboveCurrent();
    }
    private void InstantiateNewLevel()
    {
        Destroy(_currentLevel);

        _currentLevel = _nextLevel;
        _nextLevel = Instantiate(GetRandomUnusedLevel(), transform);

        _nextLevel.GetComponent<ScrollVertically>().RecalculateSpeed();

        PlaceNextLevelAboveCurrent();
    }

    private void PlaceNextLevelAboveCurrent()
    {
        float nextLevelOffsetY = _currentLevel.GetComponent<LevelInfo>().BridgePositionY + 0.9f; //Overlap a bit the two TileMaps to avoid empy spaces between the current and next level.
        _nextLevel.transform.position = new Vector3(0.0f, nextLevelOffsetY, 0.0f);
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
