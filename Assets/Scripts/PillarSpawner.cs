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
     private float yMax = 40;

     private float xPosMin = 5;
     private float xPosMax = 20;

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
            GameObject newPillar = Instantiate(pillarPrefab, new Vector3(1, 1, 3), this.transform.rotation);
            //scale
            newPillar.transform.localScale = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 3);
            //adjust pos
            float newX =  lastPos + newPillar.transform.localScale.x + Random.Range(xPosMin, xPosMax) + 10;
            newPillar.transform.position = new Vector3(newX, newPillar.transform.localScale.y/2, newPillar.transform.position.z);
            //rename
            newPillar.name = "Pillar";
            float col = Random.Range(0.1f, 0.9f);
            Color color = new Color(col, col, col);
            Renderer pillarRenderer = newPillar.GetComponent<Renderer>();
            pillarRenderer.material.SetColor("_Color", color);
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
