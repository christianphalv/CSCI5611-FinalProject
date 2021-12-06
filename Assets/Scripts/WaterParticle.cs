using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour {

    // --- Properties ---
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public float dens;
    [HideInInspector]
    public float densN;
    [HideInInspector]
    public float press;
    [HideInInspector]
    public float pressN;

    private float radius;
    private float K_restitution = 0.8f;


    // --- Methods ---



    // Start is called before the first frame update
    void Start() {
        radius = this.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update() {

    }


    private void OnCollisionEnter(Collision collision) {
        float d = radius - (collision.GetContact(0).point - transform.position).magnitude;
        Vector3 normal = collision.GetContact(0).normal.normalized;
        transform.position += normal * d;
        velocity -= 2 * K_restitution * Vector3.Dot(velocity, normal) * normal;
    }

    private void OnCollisionStay(Collision collision) {
        float d = radius - (collision.GetContact(0).point - transform.position).magnitude;
        Vector3 normal = collision.GetContact(0).normal.normalized;
        transform.position += normal * d;
    }
}
