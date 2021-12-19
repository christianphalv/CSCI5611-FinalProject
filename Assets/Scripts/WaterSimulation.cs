using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSimulation : MonoBehaviour {

    // --- Properties ---
    public WaterParticle particlePrefab;
    [SerializeField] private Spawner spawner;
    private WaterParticle[] particles;//make an array of particles, each a linked list of neighbors
    private Vector3 gravity = new Vector3(0f, -9.8f, 0f);
    private float radius;

    // K Constants
    public float h = 0.01f;
    public float K_stiff = 10f;
    public float nearK_stiff = 10f;
    public float K_restDensity = 10f;
    public float K_spring = 0.5f;
    public float L = 0.005f; //spring rest length, less than h



    // --- Methods ---
    public void Initialize() {
        particles = new WaterParticle[100];//spawner.max];
        radius = particlePrefab.gameObject.transform.localScale.x / 2;

    }
    public void createParticle(Vector3 position, int index) {
        WaterParticle newParticle = Instantiate(particlePrefab, position, Quaternion.identity);
        particles[index] = newParticle;
    }

    private void updateParticles() {
        // Add gravity force, save previous positions, AND advance to predicted positions
        foreach(WaterParticle p in particles){
            if(p != null){
                p.velocity += gravity * Time.deltaTime;
                p.prevPosition = p.transform.position;
                p.transform.position += p.velocity * 0.9f * Time.deltaTime;//damping included
                WaterParticle ptr = p.nextParticle;
                while(ptr != null){
                    if(Vector3.Magnitude(p.transform.position - ptr.transform.position) > h){
                        //remove particle from list of neighbors
                        if(ptr.nextParticle != null){
                            ptr.nextParticle.prevParticle = ptr.prevParticle;
                        }
                        else{//if at end of list then remove self
                            ptr.prevParticle.nextParticle = null;
                        }
                    }
                    ptr = ptr.nextParticle;
                }
            }

        }
 

        //everything below (until velocity update) ONLY will occur between a particle and its neighbors
        //Apply viscosity -- would be in between apply grav and move particles
        // foreach(ParticlePair pair in pairs){


        // }

        //ONLY DO THESE PARTS for particles affected by main particle
        //adjust Springs

        //applySpring Displacments, ELASTICITY
        foreach(WaterParticle p in particles){
            if(p!= null){
                WaterParticle ptr = p.nextParticle;
                while(ptr!= null){
                    float r = (p.transform.position - ptr.transform.position).magnitude;
                    Vector3 normal = (p.transform.position - ptr.transform.position).normalized;
                    Vector3 displacement = Mathf.Pow(Time.deltaTime, 2) * K_spring * (1 - L/h) * (L - r) * normal;
            //CHECK HERE about displacements?
                    p.transform.position -= displacement/2;
                    ptr.transform.position += displacement/2;


                    float q = 1 - ((p.transform.position - ptr.transform.position).magnitude / h);
                    float q2 = Mathf.Pow(q, 2);
                    float q3 = Mathf.Pow(q, 3);
                    p.density += q2;
                    ptr.density += q2;
                    p.nearDensity += q3;
                    ptr.nearDensity += q3;


                    float pressure = p.pressure + ptr.pressure;
                    float nearPressure = p.nearPressure + ptr.nearPressure;
                    float disp = (pressure * q2 + nearPressure * q3) * Mathf.Pow(Time.deltaTime, 2);
                    normal = (p.transform.position - ptr.transform.position).normalized;
                
                    //update differently here!
                    p.velocity += disp * normal;
                    p.velocity -= disp * normal;
                    
                    ptr = ptr.nextParticle;
                }

                p.pressure = K_stiff * (p.density - K_restDensity);
                p.nearPressure = nearK_stiff * p.nearDensity;

            }
            
       }

        //DOUBLE DENSITY RELAXATION - start
        // Update densities
        // foreach(WaterParticle p in particles){
        //     if(p != null){
        //         WaterParticle ptr = p.nextParticle;
        //         while(ptr!= null){
                    

        //             ptr = ptr.nextParticle;
        //         }
        //     }
            
        // }

        // Update pressures
        // foreach (WaterParticle p in particles) {
        //     if(p!= null){
        //         p.pressure = K_stiff * (p.density - K_restDensity);
        //         p.nearPressure = nearK_stiff * p.nearDensity;
        //     }
        // }

        // Add pressure force
        // foreach(WaterParticle p in particles){
        //     if(p!= null){
        //         WaterParticle ptr = p.nextParticle;
        //         while(ptr!= null){
        //             float q = 1 - ((p.transform.position - ptr.transform.position).magnitude / h);
        //             float q2 = Mathf.Pow(q, 2);
        //             float q3 = Mathf.Pow(q, 3);
        //             float pressure = p.pressure + ptr.pressure;
        //             float nearPressure = p.nearPressure + ptr.nearPressure;
        //             float displacement = (pressure * q2 + nearPressure * q3) * Mathf.Pow(Time.deltaTime, 2);
        //             Vector3 normal = (p.transform.position - ptr.transform.position).normalized;
                
        //             //update differently here!
        //             p.velocity += displacement * normal;
        //             p.velocity -= displacement * normal;
        //             //also like how is this affecting the sim? vel is reset in next few lines

        //             ptr = ptr.nextParticle;
        //         }
        //     }
            
        // }
        ////DOUBLE DENSITY RELAXATION - end

        //update neighbors
        


        //Find/update velocity of neighbors of each particle
        foreach (WaterParticle p in particles){//CHANGE TO +=, LOOKED PROMISING!
            if(p!= null){
                p.velocity = (p.transform.position - p.prevPosition) / Time.deltaTime;
            }
        }
    }


    private void updateNeighbors(){
        for(int i = 0; i < particles.Length/2; i++){
            if(particles[i] != null){
                for(int j = 0; j < particles.Length; j++){
                    if(particles[j] != null){
                        if(i!=j && Vector3.Magnitude(particles[i].transform.position - particles[j].transform.position) < h){
                            if(particles[i].nextParticle!= null){
                                particles[i].nextParticle.prevParticle = particles[j];
                            }
                            if(particles[j].nextParticle != null){
                                particles[j].nextParticle.prevParticle = particles[i];
                            }
                            particles[i].nextParticle = particles[j];
                            particles[j].nextParticle = particles[i];
                        }
                    }
                }
            } 
        }
    }

    private void FixedUpdate() {
        if(spawner.finished){
            updateParticles();
            updateNeighbors();
        }
    }
}