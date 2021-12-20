using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverItems;
    [SerializeField] TrailRenderer trail;
    public bool started;

    public void MoveToStart(){
        transform.position = new Vector3(0, 20.5f, 0);
        trail.enabled = true;
       // this.GetComponent<Rigidbody>().isKinematic = false;
        started = true;
    }

    public void EndGame(){//called when quit is pressed
        started = false;
        trail.enabled = false;
        transform.position = new Vector3(0, 20.5f, 0);
        gameOverItems.SetActive(false);
    }

    public void GameOver(){//when player gets hit
        started = false;
        this.GetComponent<Rigidbody>().velocity = Vector3.down;
        trail.enabled = false;
        gameOverItems.SetActive(true);
    }
}
