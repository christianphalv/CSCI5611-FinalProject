using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour {

    // --- Properties ---
    [HideInInspector]
    public Vector3 prevPosition;
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public float density;
    [HideInInspector]
    public float nearDensity;
    [HideInInspector]
    public float pressure;
    [HideInInspector]
    public float nearPressure;

    private float radius;
    private float k_restitution = .7f;
    private Vector3 gravity = new Vector3(0f, -9.8f, 0f);

    // --- Methods ---
    void Start() {
        radius = this.GetComponent<SphereCollider>().radius;
    }

     public void OnCollisionEnter(Collision collision) {
        float d = radius - (collision.GetContact(0).point - transform.position).magnitude;
        Vector3 normal = collision.GetContact(0).normal.normalized;
        velocity *= -k_restitution;
     }

    public void OnCollisionStay(Collision collision) {
        velocity -= gravity * Time.deltaTime;
    }
}
