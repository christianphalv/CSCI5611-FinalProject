using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Tuning Parameters")]
    public float _speed;

    // Private properties
    private Vector3 _velocity;
    private PlayerController _playerController;

    void Start() {
        _velocity = new Vector3(-_speed, 0f, 0f);
        _playerController = FindObjectOfType<PlayerController>();
    }


    void Update() {
        this.transform.position += _velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Player>()) {
            _playerController.GameOver();
        }
    }

    public void initializeProjectile(float speed) {
        _speed = speed;
        _velocity = new Vector3(-_speed, 0f, 0f);
    }
}
