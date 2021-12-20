using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rope : MonoBehaviour {

    [Header("Rope Parameters")]
    public int _numNodes = 8;

    [Header("Constants")]
    public float K_gravity = 10f;
    private float K_restLength;
    public float K_springForce = 50f;
    public float K_dampForce = 20f;

    // Rope data
    private Vector3[] _positions;
    private Vector3[] _velocities;

    // Private properties
    private Vector3 _gravity;
    private LineRenderer _lineRenderer;

    void Start() {
        
    }


    void Update() {

        // Render the rope using a line renderer
        renderRope();
        
    }

    private void FixedUpdate() {

        // Update rope simulation
        updateRope(Time.deltaTime);
        //updateEndObject();
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < _numNodes; i++) {
            Gizmos.DrawSphere(_positions[i], 0.05f);
        }
    }

    public void initializeRope(Vector3 playerPosition, Vector3 initialVelocity) {

        // Initialize properties
        _positions = new Vector3[_numNodes];
        _velocities = new Vector3[_numNodes];
        _gravity = new Vector3(0f, -K_gravity, 0f);
        _lineRenderer = GetComponent<LineRenderer>();

        // Calculate initial rotation quaternion
        Vector3 toPlayer = playerPosition - this.transform.position;
        Quaternion initialRotation = Quaternion.FromToRotation(Vector3.down, toPlayer);

        // Calculate rest length
        K_restLength = toPlayer.magnitude / (_numNodes - 1);

        // Setup rope
        for (int i = 0; i < _numNodes; i++) {
            _positions[i] = this.transform.position - new Vector3(0f, i * K_restLength, 0f);
            _positions[i] = rotatePointAboutPivot(_positions[i], this.transform.position, initialRotation);
            _velocities[i] = new Vector3(0f, 0f, 0f);
        }

        _velocities[_numNodes - 1] = initialVelocity;

        // Setup line renderer
        _lineRenderer.positionCount = _numNodes;
    }

    public RopeNode getEndNode() {
        RopeNode node = new RopeNode();
        node.position = _positions[_numNodes - 1];
        node.velocity = _velocities[_numNodes - 1];
        return node;
    }

    private void updateRope(float dt) {

        // Update velocities
        for (int i = 0; i < _numNodes; i++) {

            // Update internal velocities
            if (i < _numNodes - 1) {

                // Calculate e and l
                Vector3 e = _positions[i + 1] - _positions[i];
                float l = e.magnitude;
                e = e.normalized;

                // Calculate v1 and v2
                float v1 = Vector3.Dot(e, _velocities[i]);
                float v2 = Vector3.Dot(e, _velocities[i + 1]);

                // Calculate internal forces
                float f = -K_springForce * (K_restLength - l) - K_dampForce * (v1 - v2);

                // Update velocities with internal forces
                _velocities[i] += f * e * dt;
                _velocities[i + 1] -= f * e * dt;
            }

            // Update velocities with gravitational forces
            _velocities[i] += _gravity * dt;
        }

        // Fix top node
        _velocities[0] = new Vector3(0f, 0f, 0f);
        _positions[0] = this.transform.position;

        // Update positions
        for (int i = 1; i < _numNodes; i++) {
            _positions[i] += _velocities[i] * dt;
        }
    }

    private void renderRope() {
        _lineRenderer.SetPositions(_positions);
    }

    /*
    private void updateEndObject() {


        if (_endObject) {
            _endObject.transform.position = _positions[_numNodes - 1];
            _endObject.GetComponent<Rigidbody>().velocity = _velocities[_numNodes - 1];
        }
    }
    */

    private Vector3 rotatePointAboutPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        return rotation * (point - pivot) + pivot;
    }
}

public struct RopeNode {
    public Vector3 position;
    public Vector3 velocity;
}