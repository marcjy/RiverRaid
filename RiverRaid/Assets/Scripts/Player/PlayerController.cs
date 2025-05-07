using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 2.0f;

    private InputManager _inputManager;
    private float _moveDirection = 0.0f;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputManager.OnPlayerMoves += HandlePlayerMoves;
    }


    // Update is called once per frame
    void Update()
    {
        MoveHorizontally();
    }
    private void HandlePlayerMoves(object sender, float direction) => _moveDirection = direction;
    private void MoveHorizontally() => transform.position += new Vector3(_moveDirection, 0.0f, 0.0f) * MoveSpeed * Time.deltaTime;
}
