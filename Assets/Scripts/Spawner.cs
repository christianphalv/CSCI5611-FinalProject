using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public WaterSimulation simulation;
    private float timer;
    private int max = 300;
    private int count = 0;
    private float v = 0.1f;


    // Start is called before the first frame update
    void Start() {
        timer = 1f;
    }

    // Update is called once per frame
    void Update() {
        if (count < max && timer <= 0f) {
            float dx = Random.Range(-v, v);
            float dy = Random.Range(-v, v);
            float dz = Random.Range(-v, v);
            simulation.createParticle(transform.position + new Vector3(2f + dx, dy, dz));
            count++;
            timer = 0.01f;
        }
        timer -= Time.deltaTime;
    }
}
