using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] LevelPrefabs;

    private GameObject _currentLevel;
    private GameObject _nextLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentLevel = Instantiate(LevelPrefabs[0], transform);
        _nextLevel = Instantiate(LevelPrefabs[1], transform);

        float nextLevelOffsetY = _currentLevel.GetComponent<LevelInfo>().BridgePositionY + 1;
        _nextLevel.transform.position = new Vector3(0.0f, nextLevelOffsetY, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
