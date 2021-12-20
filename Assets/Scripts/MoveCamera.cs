using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void LateUpdate(){
       if(player.GetComponent<PlayerController>().started){
         transform.position = new Vector3(player.transform.position.x + 10, 20, transform.position.z);
       }
    }
   public void MoveCameraToStart(){
       StartCoroutine(MoveToStart());
   }
   IEnumerator MoveToStart(){
       float time = 0;
       float duration = 1f;
       Vector3 start = transform.position;
       Vector3 goal = new Vector3(15, 20, -13);
       while(time < duration){
            transform.position = Vector3.Lerp(start, goal, time / duration);
            time += Time.deltaTime;
            yield return null;
       }
       transform.position = goal;
   }

   public void MoveCameraToTitle(){
       transform.position = new Vector3(transform.position.x, 90.5f, -13);
   }

   IEnumerator MoveToTitle(){
       float time = 0;
       float duration = 1f;
       Vector3 start = transform.position;
       Vector3 goal = new Vector3(transform.position.x, 90.5f, -13);
       while(time < duration){
           transform.position = Vector3.Lerp(start, goal, time / duration);
            time += Time.deltaTime;
            yield return null;
       }
       transform.position = goal;
   }
}
