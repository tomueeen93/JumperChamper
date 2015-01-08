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
	private void OnTriggerEnter(Collider other)
	{
		if(other.tag =="Ground"){
			Debug.Log("Crash");
			Destroy(board_obj);
		}
	}
}
