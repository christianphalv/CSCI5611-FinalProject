using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Player : MonoBehaviour {

    // Public properties
    public float _movementSpeed;
    public float _jumpSpeed;
    public Transform _groundedPoint;

    // Physics properties
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private float _groundedRadius;
    private LayerMask _groundLayer;

    void Start() {

        // Initialize properties
        _rigidbody = GetComponent<Rigidbody>();
        _isGrounded = true;
        _groundedRadius = 0.01f;
        _groundLayer = LayerMask.GetMask("Ground");
    }

    void Update() {
        playerMove();
        checkGrounded();
        playerJump();
    }
    
    private void playerMove() {

        // Get movement direction
        float direction = Input.GetAxisRaw("Horizontal");

        // Set velocity direction
        _rigidbody.velocity = new Vector3(direction * _movementSpeed, _rigidbody.velocity.y, 0f);
    }

    private void playerJump() {

        // Set velocity jump
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpSpeed, 0f);
        }

    }
    
    private Quaternion getMouseRotation() {

        // Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        // Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Find the direction from the player to the mouse
        Vector2 dir = mouseOnScreen - positionOnScreen;

        // Find the angle to turn
        float mouseAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        // Create quaternion based on mouse angle
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0f, 0f, -mouseAngle));

        return mouseRotation;
    }

    private void checkGrounded() {

        // Get colliders interactions with ground
        Collider[] colliders = Physics.OverlapSphere(_groundedPoint.position, _groundedRadius, _groundLayer);

        // Set grounded if any ground colliders
        if (colliders.Length > 0) {
            _isGrounded = true;
        } else {
            _isGrounded = false;
        }
    }
}
