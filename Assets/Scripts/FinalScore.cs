using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    public Text finalScore;
    public Text score;

    void Update()
    {
        finalScore.text = "Score = " + score.text;
        score.enabled = false;
    }
}
