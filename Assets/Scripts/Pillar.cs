// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Pillar : MonoBehaviour
// {
//     private float xMin = 5;
//     private float xMax = 10;
//     private float yMin = 20;
//     private float yMax = 35;

//     private float xPosMin = 7;
//     private float xPosMax = 17;

//     private bool spawned = false;
//     public bool canSpawn;
//     public float xStart;

//     private GameObject player;

//     void Awake(){
//         BoxCollider collider = GetComponent<BoxCollider>();
//         //collider.size = new Vector3(collider.size.x, 100, collider.size.z);
//         canSpawn = true;
//         xStart = 5;
//     }

//     void Update(){
//         if(player != null){
//             if(player.transform.position.x - transform.position.x > 50){
//                 Destroy(this.gameObject);
//             }
//         }
//     }

//     void OnTriggerEnter(Collider other){
//         if(other.gameObject.CompareTag("Player")){
//             player = other.gameObject;
//             if(!spawned && canSpawn){//spawn next 3
//                  //first tower
//                 float currentX = xStart;
//                 float newX = currentX  + transform.localScale.x;
//                 GameObject newPillar = Instantiate(this.gameObject, new Vector3(newX, 5, 3), this.transform.rotation);
//                 //scale
//                 newPillar.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
//                 //adjust pos
//                 newX +=  newPillar.transform.localScale.x + Random.Range(xPosMin, xPosMax);
//                 newPillar.transform.position = new Vector3(newX, newPillar.transform.localScale.y/2, newPillar.transform.position.z);
//                 //rename
//                 newPillar.name = "Pillar";
//                 newPillar.GetComponent<Pillar>().canSpawn = true;

//                 //second tower
//                 GameObject newPillar2 = Instantiate(this.gameObject, new Vector3(newX, 5, 3), this.transform.rotation);
//                 //scale
//                 newPillar2.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
//                 //adjust pos
//                 newX +=  newPillar2.transform.localScale.x + Random.Range(xPosMin, xPosMax);
//                 newPillar2.transform.position = new Vector3(newX, newPillar2.transform.localScale.y/2, newPillar2.transform.position.z);
//                 //rename
//                 newPillar2.name = "Pillar";
//                 newPillar2.GetComponent<Pillar>().canSpawn = false;

//                 //third tower
//                 GameObject newPillar3 = Instantiate(this.gameObject, new Vector3(newX, 5, 3), this.transform.rotation);
//                 //scale
//                 newPillar3.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
//                 //adjust pos
//                 newX +=  newPillar3.transform.localScale.x + Random.Range(xPosMin, xPosMax);
//                 newPillar3.transform.position = new Vector3(newX, newPillar3.transform.localScale.y/2, newPillar3.transform.position.z);
//                 //rename
//                 newPillar3.name = "Pillar";
//                 newPillar3.GetComponent<Pillar>().canSpawn = false;
            
//                 spawned = true;
//                 newPillar.GetComponent<Pillar>().xStart = newPillar3.transform.position.x;
//             }         
//         }
//     }
// }
