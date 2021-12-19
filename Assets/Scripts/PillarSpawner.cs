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
        GameObject firstPillar = Instantiate(pillarPrefab, new Vector3(10, 5, 3), this.transform.rotation);
        firstPillar.transform.localScale = new Vector3(8, 30, 3);
        firstPillar.transform.position = new Vector3(firstPillar.transform.position.x, firstPillar.transform.localScale.y/2, firstPillar.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
