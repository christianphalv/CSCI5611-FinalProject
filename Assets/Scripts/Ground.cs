using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Transform player;
    float xPos;
    void Update()
    {
        xPos = player.position.x;
        transform.position = new Vector3(xPos + 30, transform.position.y, transform.position.z);
    }
}
