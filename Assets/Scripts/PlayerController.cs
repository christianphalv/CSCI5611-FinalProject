using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int speed = 10;

    public bool started;

    // Update is called once per frame
    void Update()
    {
        if(started){
            if(Input.GetKey(KeyCode.RightArrow)){
                gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.LeftArrow)){
                gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }

    public void StartGame(){
        started = true;
    }

    public void MoveToStart(){
        transform.position = new Vector3(0, 20.5f, 0);
    }
}
