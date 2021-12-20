using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pillarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate first pillar (same x pos each round)
        // GameObject firstPillar = Instantiate(pillarPrefab, new Vector3(10, 5, 3), this.transform.rotation);
        // firstPillar.transform.localScale = new Vector3(8, 30, 3);
        // firstPillar.transform.position = new Vector3(firstPillar.transform.position.x, firstPillar.transform.localScale.y/2, firstPillar.transform.position.z);
    }

    public void CreateStartingPillars(){//create starting pillars
                float xMin = 5;
                float xMax = 10;
                float yMin = 20;
                float yMax = 35;
                float xPosMin = 7;
                float xPosMax = 17;

        //first tower
                float currentX = transform.position.x;
                float newX = currentX  + transform.localScale.x;
                GameObject newPillar = Instantiate(pillarPrefab, new Vector3(newX, 5, 3), this.transform.rotation);
                //scale
                newPillar.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
                //adjust pos
                newX +=  newPillar.transform.localScale.x + Random.Range(xPosMin, xPosMax);
                newPillar.transform.position = new Vector3(newX, newPillar.transform.localScale.y/2, newPillar.transform.position.z);
                //rename
                newPillar.name = "Pillar";
                newPillar.GetComponent<Pillar>().canSpawn = true;

                //second tower
                GameObject newPillar2 = Instantiate(pillarPrefab, new Vector3(newX, 5, 3), this.transform.rotation);
                //scale
                newPillar2.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
                //adjust pos
                newX +=  newPillar2.transform.localScale.x + Random.Range(xPosMin, xPosMax);
                newPillar2.transform.position = new Vector3(newX, newPillar2.transform.localScale.y/2, newPillar2.transform.position.z);
                //rename
                newPillar2.name = "Pillar";
                newPillar2.GetComponent<Pillar>().canSpawn = false;

                //third tower
                GameObject newPillar3 = Instantiate(pillarPrefab, new Vector3(newX, 5, 3), this.transform.rotation);
                //scale
                newPillar3.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
                //adjust pos
                newX +=  newPillar3.transform.localScale.x + Random.Range(xPosMin, xPosMax);
                newPillar3.transform.position = new Vector3(newX, newPillar3.transform.localScale.y/2, newPillar3.transform.position.z);
                //rename
                newPillar3.name = "Pillar";
                newPillar3.GetComponent<Pillar>().canSpawn = false;
            
                newPillar.GetComponent<Pillar>().xStart = newPillar3.transform.position.x;

    }

    public void DestroyAllPillars(){
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Pillar");
        foreach  (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
    }


}
