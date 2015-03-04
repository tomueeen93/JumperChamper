using UnityEngine;
using System.Collections;

public class ObjectDestroyerController : MonoBehaviour
{
    GameObject board_obj;
    GameObject score_obj;

    void Start()
    {
        board_obj = GameObject.Find("Board");
        score_obj = GameObject.Find("ScoreObject");
    }

    private void OnCollisionEnter(Collision other)
    {
        // クラッシュした時の処理
        Debug.Log("Crash");
        board_obj.gameObject.GetComponent<BoardController>().crashed = true;
        // Destroy(board_obj);
        Debug.Log("wait start");
        float s = board_obj.gameObject.GetComponent<BoardController>().score;
        score_obj.gameObject.GetComponent<ScoreController>().score = s;
        DontDestroyOnLoad(score_obj.gameObject);
        Application.LoadLevel("score");
    }
}
