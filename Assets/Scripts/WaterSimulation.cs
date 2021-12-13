using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSimulation : MonoBehaviour {

    // --- Properties ---
    public WaterParticle particlePrefab;
    private List<WaterParticle> particles;
    private Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    private float radius;

    // K Constants
    public float h = 0.01f;
    public float K_stiff = 10f;
    public float nearK_stiff = 10f;
    public float K_restDensity = 10f;
    //public float K_restitution = 0.8f;



    // --- Methods ---
    public void createParticle(Vector3 position) {
        WaterParticle newParticle = Instantiate(particlePrefab, position, Quaternion.identity);
        particles.Add(newParticle);
    }

    private void updateParticles() {
        // Create pairs
        List<ParticlePair> pairs = new List<ParticlePair>(); ;
        for (int i = 0; i < particles.Count; i++) {
            for (int j = i; j < particles.Count; j++) {
                if ((particles[i].transform.position - particles[j].transform.position).magnitude < h) {
                    pairs.Add(new ParticlePair(i, j));
                }
            }
        }

        // Add gravity force, save previous positions, AND advance to predicted positions
        foreach (WaterParticle p in particles) {
            p.velocity += gravity * Time.deltaTime;
            p.prevPosition = p.transform.position;
            p.transform.position += p.velocity * 0.9f * Time.deltaTime;//damping included
        }

        //Apply viscosity
        // foreach(ParticlePair pair in pairs){


        // }


        //adjust Springs

        //applySpring Displacments

        //DOUBLE DENSITY RELAXATION - start
        // Update densities
        foreach (ParticlePair pair in pairs) {
            pair.q = 1 - ((particles[pair.p1].transform.position - particles[pair.p2].transform.position).magnitude / h);
            pair.q2 = Mathf.Pow(pair.q, 2);
            pair.q3 = Mathf.Pow(pair.q, 3);
            particles[pair.p1].density += pair.q2;
            particles[pair.p2].density += pair.q2;
            particles[pair.p1].nearDensity += pair.q3;
            particles[pair.p2].nearDensity += pair.q3;
        }

        // Update pressures
        foreach (WaterParticle p in particles) {
            p.pressure = K_stiff * (p.density - K_restDensity);
            p.nearPressure = nearK_stiff * p.nearDensity;
        }

        // Add pressure force
        foreach (ParticlePair pair in pairs) {
            float pressure = particles[pair.p1].pressure + particles[pair.p2].pressure;
            float nearPressure = particles[pair.p1].nearPressure + particles[pair.p2].nearPressure;
            float displacement = (pressure * pair.q2 + nearPressure * pair.q3) * Mathf.Pow(Time.deltaTime, 2);
            Vector3 normal = (particles[pair.p1].transform.position - particles[pair.p2].transform.position).normalized;
            particles[pair.p1].velocity += displacement * normal;
            particles[pair.p2].velocity -= displacement * normal;
        }
        ////DOUBLE DENSITY RELAXATION - end

        // Update velocities
        foreach (WaterParticle p in particles) {
            p.velocity = (p.transform.position - p.prevPosition) / Time.deltaTime;
        }
    }


    // Start is called before the first frame update
    void Start() {
        particles = new List<WaterParticle>();
        radius = particlePrefab.gameObject.transform.localScale.x / 2;

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