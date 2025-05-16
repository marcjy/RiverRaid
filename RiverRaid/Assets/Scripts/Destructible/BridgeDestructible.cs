using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeDestructible : BaseDestructible
{
    private BridgeManager _bridgeManager;
    private Tilemap _tileMap;

    private void Start()
    {
        _bridgeManager = GetComponent<BridgeManager>();
        _tileMap = GetComponent<Tilemap>();
    }

    protected override void OnDestroyedInternal(GameObject source)
    {
        float offsetPositionY = 0.5f;
        float offsetPositionX = 0.5f;

        foreach (Vector3Int position in _bridgeManager.GetDestructibleTiles())
        {
            DestroyOnAnimationEvent explosion = Instantiate(DestroyAnimation);
            explosion.transform.position = new Vector3(position.x + offsetPositionX, source.transform.position.y + offsetPositionY);
            explosion.GetComponent<ScrollVertically>().RecalculateSpeed();

            _tileMap.SetTile(position, null);
        }
    }
}
