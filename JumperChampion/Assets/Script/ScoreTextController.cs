using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextController : MonoBehaviour {
    GameObject score_object;
    public float score = 0;

    void Start () {
        score_object = GameObject.Find("ScoreObject");
        // Debug.Log(score_object.GetComponent<ScoreController>().score);
        // Debug.Log(gameObject.GetComponent<Text>().text);
        setScore();
        setMessage();
	}

    void setScore()
    {
        score = score_object.GetComponent<ScoreController>().score;
    }
    void setMessage()
    {
        string first_str = "Score\n";
        string secound_str = score+ " pt\n";
        string third_str = "";
        if (score >= 1000) third_str = "GREAT !!";
        else if (score >= 500) third_str = "GOOD !";
        else third_str = "BAD :(";

        string message = first_str + secound_str + third_str;
        gameObject.GetComponent<Text>().text = message;
    }
}
