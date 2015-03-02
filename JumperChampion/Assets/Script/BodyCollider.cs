using UnityEngine;
using System.Collections;

public class BodyCollider : MonoBehaviour {
	GameObject board_obj;

	// Use this for initialization
	void Start () {
		 board_obj = GameObject.Find("Board");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// A body touch ground
	private IEnumerator OnTriggerEnter(Collider other)
	{
		if(other.tag =="Ground"){
			Debug.Log("Crash");
			board_obj.rigidbody.AddForce(new Vector3(0,100,0),ForceMode.Impulse);
			board_obj.transform.FindChild("Human").transform.FindChild("Capsule").collider.isTrigger = false;
			board_obj.transform.FindChild("Human").transform.FindChild("Sphere").collider.isTrigger = false;
			board_obj.gameObject.GetComponent<BoardController>().crashed = true;
			// Destroy(board_obj);
			Debug.Log ("wait start");
			yield return new WaitForSeconds(2);
			Application.LoadLevel("title");

		}
	}

}
