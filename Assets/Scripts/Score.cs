using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public float score;
    public float scoreIncrease;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0f;
        scoreIncrease = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = (int)score + "";
        score += scoreIncrease * Time.deltaTime;
    }
}
