using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public WaterSimulation simulation;
    private float timer;
    public int max = 500;
    private int count = 0;
    private float v = 0.8f;
    public bool finished = false;


    // Start is called before the first frame update
    void Start() {
        timer = 1f;
        simulation.Initialize();
        //max = 500;


    }

    // Update is called once per frame
    void Update() {
        max = 100;
        if (count < max && timer <= 0f) {
            float dx = Random.Range(-v, v);
            float dy = Random.Range(-v, v);
            float dz = Random.Range(-v, v);
            simulation.createParticle(transform.position + new Vector3(2f + dx, dy, dz), count);
            count++;
            timer = 0.01f;
        }
        else{
            finished = true;
           // Debug.Log("DONE! Count: " + count);
        }
        timer -= Time.deltaTime;
    }
}
