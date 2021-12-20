using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + player.transform.position.x.ToString("0");   
    }
}
