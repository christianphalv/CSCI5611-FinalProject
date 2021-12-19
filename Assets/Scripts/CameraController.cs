using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Public properties
    public Transform _player;
    public float _zoom = 10;

    // Private properties
    private Vector3 _cameraOffset;

    void Start() {
        _cameraOffset = new Vector3(0f, 0f, -_zoom);
    }

    void Update() {
        this.transform.position = _player.position + _cameraOffset;
    }
}
