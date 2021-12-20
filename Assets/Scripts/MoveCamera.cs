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
       Vector3 start = transform.localPosition;
       Vector3 goal = new Vector3(15, 0, -13);
       Vector3 startRot = transform.eulerAngles;
       Vector3 goalRot = new Vector3(8, -1, 0);
       while(time < duration){
           transform.localPosition = Vector3.Lerp(start, goal, time / duration);
           transform.eulerAngles = Vector3.Lerp(startRot, goalRot, time/duration);
            time += Time.deltaTime;
            yield return null;
       }
       transform.localPosition = goal;
       transform.eulerAngles = goalRot;
   }

   public void MoveCameraToTitle(){
       StartCoroutine(MoveToTitle());
   }

   IEnumerator MoveToTitle(){
       float time = 0;
       float duration = 1f;
       Vector3 goal = transform.localPosition;
       Vector3 start = new Vector3(15, 0, -13);
       Vector3 goalRot = transform.eulerAngles;
       Vector3 startRot = new Vector3(8, -1, 0);
       while(time < duration){
           transform.localPosition = Vector3.Lerp(start, goal, time / duration);
           transform.eulerAngles = Vector3.Lerp(startRot, goalRot, time/duration);
            time += Time.deltaTime;
            yield return null;
       }
       transform.localPosition = goal;
       transform.eulerAngles = goalRot;
   }
}
