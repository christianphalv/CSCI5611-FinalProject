using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rope : MonoBehaviour {

    [Header("Rope Parameters")]
    public int _numNodes;

    [Header("Constants")]
    public float K_gravity = 10f;
    public float K_restLength = 2f;
    public float K_springForce = 50f;
    public float K_dampForce = 20f;

    // Rope data
    private Vector3[] _positions;
    private Vector3[] _velocities;
    private Vector3[] _accelerations;

    // Private properties
    private Vector3 _gravity;
    private LineRenderer _lineRenderer;

    void Start() {

        // Initialize properties
        _positions = new Vector3[_numNodes];
        _velocities = new Vector3[_numNodes];
        _accelerations = new Vector3[_numNodes];
        _gravity = new Vector3(0f, -K_gravity, 0f);
        _lineRenderer = GetComponent<LineRenderer>();

        // Setup rope
        for (int i = 0; i < _numNodes; i++) {
            _positions[i] = this.transform.position - new Vector3(0f, i * K_restLength, 0f);
            _positions[i] = rotatePointAboutPivot(_positions[i], this.transform.position, this.transform.rotation);
            _velocities[i] = this.transform.position;
        }

        // Setup line renderer
        _lineRenderer.positionCount = _numNodes - 1;
    }


    void Update() {

        // Render the rope using a line renderer
        renderRope();
        
    }

    private void FixedUpdate() {

        // Update rope simulation
        updateRope(Time.deltaTime);
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < _numNodes; i++) {
            Gizmos.DrawSphere(_positions[i], 0.05f);
        }
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

        // Update positions
        for (int i = 0; i < _numNodes; i++) {
            _positions[i] += _velocities[i] * dt;
        }
    }

    private void renderRope() {

        // Update positions
        _lineRenderer.SetPositions(_positions);
    }

    private Vector3 rotatePointAboutPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        return rotation * (point - pivot) + pivot;
    }
}
