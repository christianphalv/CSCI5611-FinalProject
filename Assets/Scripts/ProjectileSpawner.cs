using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

    public Projectile _projectilePrefab;

    [Header("Tuning Parameters")]
    public float _fireRate;
    public float _maxProjectileSpeed;
    public float _minProjectileSpeed;

    private Camera _mainCamera;
    private float _timer;
    private PlayerController _playerController;

    void Start() {

        // Initialize properties
        _mainCamera = Camera.main;
        _timer = _fireRate;
        _playerController = FindObjectOfType<PlayerController>();

    }


    void Update() {

        if (_playerController.started) {

            if (_timer > 0) {
                _timer -= Time.deltaTime;
            } else {
                spawnProjectile();
                _timer = _fireRate;
            }
        }
    }

    public void DestroyProjectiles(){//gets called on restart or quit
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach  (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
    }

    private void spawnProjectile() {


        // Calculate point just off screen
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0f));
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        float xyIntersection;
        xyPlane.Raycast(ray, out xyIntersection);
        Vector3 screenTopRight = ray.GetPoint(xyIntersection);


        // Generate random position
        Vector3 randomHeight = new Vector3(0f, Random.Range(0f, screenTopRight.y), 0f);
        Vector3 screenOffset = new Vector3(1f, 0f, 0f);
        Vector3 spawnPosition = screenTopRight + screenOffset - randomHeight;

        // Generate random speed
        float speed = Random.Range(_minProjectileSpeed, _maxProjectileSpeed);

        // Initialize projectile
        Projectile projectile = Instantiate<Projectile>(_projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.initializeProjectile(speed);
    }
}
