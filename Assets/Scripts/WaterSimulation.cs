using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSimulation : MonoBehaviour {

    // --- Properties ---
    public WaterParticle particlePrefab;
    private List<WaterParticle> particles;
    private Vector3 gravity = new Vector3(0f, -10f, 0f);
    private float radius;

    // K Constants
    private float K_smoothingRadius = 0.001f;
    private float K_stiff = 10f;
    private float K_stiffN = 10f;
    private float K_restDensity = 10f;
    private float K_restitution = 0.8f;



    // --- Methods ---
    public void createParticle(Vector3 position) {
        WaterParticle newParticle = Instantiate(particlePrefab, position, Quaternion.identity);
        particles.Add(newParticle);
    }

    private void updateParticles() {

        // Add gravity force
        foreach (WaterParticle p in particles) {
            p.velocity += gravity * Time.deltaTime;;
        }

        // Create pairs
        List<ParticlePair> pairs = new List<ParticlePair>(); ;
        for (int i = 0; i < particles.Count; i++) {
            for (int j = i; j < particles.Count; j++) {
                if ((particles[i].transform.position - particles[j].transform.position).magnitude < K_smoothingRadius) {
                    pairs.Add(new ParticlePair(i, j));
                }
            }
        }

        // Update densities
        foreach (ParticlePair pair in pairs) {
            pair.q = 1 - ((particles[pair.p1].transform.position - particles[pair.p2].transform.position).magnitude / K_smoothingRadius);
            pair.q2 = Mathf.Pow(pair.q, 2);
            pair.q3 = Mathf.Pow(pair.q, 3);
            particles[pair.p1].dens += pair.q2;
            particles[pair.p2].dens += pair.q2;
            particles[pair.p1].densN += pair.q3;
            particles[pair.p2].densN += pair.q3;
        }

        // Update pressures
        foreach (WaterParticle p in particles) {
            p.press = K_stiff * (p.dens - K_restDensity);
            p.pressN = K_stiffN * p.densN;
        }

        // Add pressure force
        foreach (ParticlePair pair in pairs) {
            float press = particles[pair.p1].press + particles[pair.p2].press;
            float pressN = particles[pair.p1].pressN + particles[pair.p2].pressN;
            float displace = (press * pair.q2 + pressN * pair.q3) * Mathf.Pow(Time.deltaTime, 2);
            Vector3 normal = (particles[pair.p1].transform.position - particles[pair.p2].transform.position).normalized;
            particles[pair.p1].velocity += displace * normal;
            particles[pair.p2].velocity -= displace * normal;
        }

        // Update positions
        foreach (WaterParticle p in particles) {
            p.transform.position += p.velocity * Time.deltaTime;
        }

        // Handle floor collisions
        foreach (WaterParticle p in particles) {
            float d = p.transform.position.y - radius;
            if (d <= 0) {
                Vector3 normal = new Vector3(0f, 1f, 0f);
                p.transform.position -= normal * d;
                p.velocity -= 2 * K_restitution * Vector3.Dot(p.velocity, normal) * normal;
            }
        }
    }


    // Start is called before the first frame update
    void Start() {
        particles = new List<WaterParticle>();
        radius = particlePrefab.gameObject.transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        updateParticles();

    }
}

public class ParticlePair {
    public int p1;
    public int p2;
    public float q;
    public float q2;
    public float q3;

    public ParticlePair(int p1, int p2) {
        this.p1 = p1;
        this.p2 = p2;
    }
}