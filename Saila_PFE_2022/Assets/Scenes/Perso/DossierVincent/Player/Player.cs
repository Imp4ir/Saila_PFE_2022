using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    // Movespeed player value
    public float moveSpeed = 0.1f;

    private Rigidbody _rbPlayer;
    private Vector3 _movement;
    
    private void Start()
    {
        _rbPlayer = gameObject.GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
    }


    private void FixedUpdate()
    {
        _rbPlayer.MovePosition(_rbPlayer.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }
}
