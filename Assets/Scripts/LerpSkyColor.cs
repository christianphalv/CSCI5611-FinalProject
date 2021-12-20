using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpSkyColor : MonoBehaviour
{
    [SerializeField] private GameObject player; // to get x val for score
    private Image image;
    private Color color1, color2;
    private float duration = 400;
    private float distance;
    
    void Start()
    {
        image = GetComponent<Image>();
        color1 = new Color(1, 0.906f, 0, 1);
        color2 = new Color(0.968f, 0, 1, 1);
        distance = 0;
        StartCoroutine(LerpImageColor(color2));
    }

    void Update(){
        distance = player.transform.position.x;        
    }

    IEnumerator LerpImageColor(Color col){
        Color startValue = image.color;
        while (distance%duration < duration)
        {
            float t  =Mathf.PingPong(distance/duration, 1);
            image.color = Color.Lerp(startValue, col, t);
            yield return null;
        }
        image.color = col;
    }
}
