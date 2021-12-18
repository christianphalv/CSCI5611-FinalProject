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
    private float mass;
    private float elasticity = 0.5f;
    private float k_restitution = .7f;
    private Vector3 gravity = new Vector3(0f, -9.8f, 0f);

    // --- Methods ---
    void Start() {
        radius = this.GetComponent<SphereCollider>().radius;
        mass = this.GetComponent<Rigidbody>().mass;
    }

//HANDLE collisions with the tub. Particle-particle collisions handles in water sim code
     public void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Tub")){
            float d = radius - (collision.GetContact(0).point - transform.position).magnitude;
            Vector3 normal = collision.GetContact(0).normal.normalized;//collision normal
            Vector3 velocityB = Vector3.zero;//collision.gameObject.GetComponent<WaterParticle>().velocity;
            float massB = 100000f;//collision.gameObject.GetComponent<WaterParticle>().mass;

            Vector3 momentum = ((1+elasticity) * Vector3.Dot((velocity - velocityB), normal)/((1/mass) + (1/massB)))*normal;
            velocity = velocity - (momentum/mass);//* k_restitution;
            //Don't update tub velocity
            //collision.gameObject.GetComponent<WaterParticle>().SetVelocity(velocityB + (momentum/massB));
        }   
     }

    public void OnCollisionStay(Collision collision) {
        velocity -= gravity * Time.deltaTime;
    }

    public void SetVelocity(Vector3 newVel){
        velocity = newVel;
    }
}
