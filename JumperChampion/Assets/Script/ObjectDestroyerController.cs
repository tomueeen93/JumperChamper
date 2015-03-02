using UnityEngine;
using System.Collections;

public class ObjectDestroyerController : MonoBehaviour {
	public void OnCollisionEnter(Collision other) {
		Destroy(other.gameObject);
		Debug.Log ("Destroy");
		Application.LoadLevel("title");
	}
}
