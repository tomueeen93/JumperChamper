using UnityEngine;
using System.Collections;

public class BodyCollider : MonoBehaviour {
	GameObject board_obj;
    GameObject score_obj;

	// Use this for initialization
	void Start () {
		board_obj = GameObject.Find("Board");
        score_obj = GameObject.Find("ScoreObject");
	}

	// A body touch ground
	private IEnumerator OnTriggerEnter(Collider other)
	{
		if(other.tag =="Ground"){
			// クラッシュした時の処理
			Debug.Log("Crash");
			board_obj.rigidbody.AddForce(new Vector3(0,100,0),ForceMode.Impulse);
			board_obj.transform.FindChild("Human").transform.FindChild("Capsule").collider.isTrigger = false;
			board_obj.transform.FindChild("Human").transform.FindChild("Sphere").collider.isTrigger = false;
			board_obj.gameObject.GetComponent<BoardController>().crashed = true;
			// Destroy(board_obj);
			Debug.Log ("wait start");
			yield return new WaitForSeconds(2);
            float s = board_obj.gameObject.GetComponent<BoardController>().score;
            score_obj.gameObject.GetComponent<ScoreController>().score = s;
            DontDestroyOnLoad(score_obj.gameObject);
			Application.LoadLevel("score");
		}
	}
}
