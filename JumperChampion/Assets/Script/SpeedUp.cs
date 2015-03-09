using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour {
	int down_power = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Enter Speed Up Area
	private void OnTriggerEnter(Collider other)
	{
		if(other.name == "Board"){
			other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * down_power, ForceMode.Impulse);
		}
	}
}
