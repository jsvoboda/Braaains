using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

    int score;

    public void IncrementScore()
    {
        score++;
    }


    // Use this for initialization
    void Start()
    {
        score = 0;
    }
	
    //// Update is called once per frame
    //void Update () {
	
    //}

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 110, 30), "Brains eaten: " + score.ToString());
    }
}
