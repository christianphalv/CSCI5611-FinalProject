using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pillarPrefab;
    [SerializeField] private PlayerController playerController;
    private Camera _mainCamera;
    private float lastPos;
     private float xMin = 5;
     private float xMax = 10;
     private float yMin = 20;
     private float yMax = 35;
     private float zMin = 2.5f;
     private float zMax = 5;

     private float xPosMin = 7;
     private float xPosMax = 17;

    void Start()
    {
        _mainCamera = Camera.main;
        lastPos = 0;
    }

    void Update(){
        if(playerController.started){
            CreatePillar();
        }
    }

    public void StartGame(){
        DestroyAllPillars();
        lastPos = 0;
    }

    public void CreatePillar(){
        // Calculate point just off screen
        Ray ray = _mainCamera.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0f));
        Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
        float xyIntersection;
        xyPlane.Raycast(ray, out xyIntersection);
        Vector3 screenTopRight = ray.GetPoint(xyIntersection);   
        
        if(screenTopRight.x - lastPos > 15){
            //spawn new pillar
            GameObject newPillar = Instantiate(pillarPrefab, new Vector3(1, 1, 1), this.transform.rotation);
            //scale
            newPillar.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
            //adjust pos
            float newX =  lastPos + newPillar.transform.localScale.x + Random.Range(xPosMin, xPosMax) + 10;
            newPillar.transform.position = new Vector3(newX, newPillar.transform.localScale.y/2, newPillar.transform.localScale.z);
            //rename
            newPillar.name = "Pillar";
            lastPos = newPillar.transform.position.x;
        }   
    }
    public void DestroyAllPillars(){
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Pillar");
        foreach  (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
        prefabs = GameObject.FindGameObjectsWithTag("Rope");
        foreach  (GameObject prefab in prefabs)
            {
                Destroy(prefab);
            }
    }


}
