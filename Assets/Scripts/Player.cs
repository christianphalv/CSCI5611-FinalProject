using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Scripting;

public class Player : MonoBehaviour {

    // Public properties
    public Transform _groundedPoint;
    public Rope _ropePrefab;

    [Header("Tuning Parameters")]
    public float _initialMovementSpeed;
    public float _jumpSpeed;
    public float _maxRopeDistance;
    public float _ropeJumpMin;
    public float _ropeJumpMax;

    // Physics properties
    private float _movementSpeed;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private float _groundedRadius;
    private LayerMask _groundLayer;
    private LayerMask _ropeAttachLayer;
    private Rope _currentRope;
    private Camera _mainCamera;

    void Start() {

        // Initialize properties
        _movementSpeed = _initialMovementSpeed;
        _rigidbody = GetComponent<Rigidbody>();
        _isGrounded = true;
        _groundedRadius = 0.01f;
        _groundLayer = LayerMask.GetMask("Ground");
        _ropeAttachLayer = LayerMask.GetMask("RopeAttach");
        _currentRope = null;
        _mainCamera = Camera.main;
    }

    void Update() {


        playerMove();
        checkGrounded();
        playerJump();
        playerRope();
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
        } else if (Input.GetKeyDown(KeyCode.Space) && _currentRope) {
            _rigidbody.velocity = new Vector3(Mathf.Clamp(_currentRope.getEndNode().velocity.x, _ropeJumpMin, _ropeJumpMax) * _movementSpeed, Mathf.Clamp(_currentRope.getEndNode().velocity.y, _ropeJumpMin, _ropeJumpMax) * _jumpSpeed, 0f);
            detachFromRope();
        }
    }

    private void playerRope() {

        if (Input.GetMouseButtonDown(0)) {
            attachToRope();

        } else if (Input.GetMouseButtonUp(0)) {
            detachFromRope();
        }

        if (_currentRope) {
            RopeNode endNode = _currentRope.getEndNode();
            this.transform.position = endNode.position;
            _rigidbody.velocity = endNode.velocity;
        }
    }

    private void attachToRope() {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _maxRopeDistance, _ropeAttachLayer)) {
            Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
            float xyIntersection;
            xyPlane.Raycast(ray, out xyIntersection);
            Vector3 hitPoint = ray.GetPoint(xyIntersection);
            //_currentRope = Instantiate<Rope>(_ropePrefab, new Vector3(hit.point.x, hit.point.y, 0f), Quaternion.identity);
            _currentRope = Instantiate<Rope>(_ropePrefab, hitPoint, Quaternion.identity);
            _currentRope.initializeRope(this.transform.position, _rigidbody.velocity);
        }
    }

    private void detachFromRope() {
        _currentRope = null;
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
